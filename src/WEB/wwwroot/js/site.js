var link = "";

modalShow = () => {
    let modal = document.querySelector('.modal');
    modal.style.display = "block";
    modal.style.paddingRight = "17px";
    modal.className = "modal fade show";
}

modalClose = () => {
    let btnShowModal = document.querySelector('.btn-modal');
    let modal = document.querySelector('.modal');
    modal.style.display = "none";
    modal.className = "modal fade";
    btnShowModal.href = link;
}


GetData = () => {
    let btnShowModal = document.querySelector('.btn-modal');
    let modalBody = document.querySelector('.modal-body');
    link = btnShowModal.href;
    btnShowModal.href = "#";

    fetch(link, {
        method: 'GET'
    })
        .then(response => response.text())
        .then(data => {
            modalBody.innerHTML = data;
            let btnSave = document.querySelector('.btn-save');
            UpdateEvent(btnSave, link);
        });
}

UpdateEvent = (btnSave, link) => {
    btnSave.addEventListener('click', (e) => {

        let modal = document.querySelector('.modal');
        let modalBody = document.querySelector('.modal-body');
        let form = document.querySelector('#form-address');
        let targetAddress = document.querySelector('#target-address');
        let data = new FormData(form);

        console.log(link);

        fetch(link, {
            method: 'POST',
            body: data
        })
            .then(response => response.text())
            .then(data => {

                modal.style.display = "none";
                modal.className = "modal fade";
                modalBody.innerHTML = "";
                targetAddress.innerHTML = data;
                loadEvents();

            });
    });
}

loadEvents = () => {
    let btnClose = document.querySelector('.btn-close');
    let btnShowModal = document.querySelector('.btn-modal');

    btnShowModal.addEventListener('click', (e) => {
        GetData();
        modalShow();
    });

    btnClose.addEventListener('click', (e) => {
        modalClose();
    });
}

loadEvents();