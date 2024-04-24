window.bootstrapModal = {
    showModal: function (modalId) {
        var modal = new bootstrap.Modal(document.getElementById(modalId));
        modal.show();
    },
    hideModal: function (modalId) {
        var modal = new bootstrap.Modal(document.getElementById(modalId));
        modal.hide();
    }
};
