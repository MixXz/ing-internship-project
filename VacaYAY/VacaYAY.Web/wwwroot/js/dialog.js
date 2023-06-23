$((function () {
    var url;
    var target;
    var redirectUrl;
    var title;
    $('body').append(`
            <div class="modal fade" id="confirmModal" tabindex="-1" role="dialog" aria-labelledby="myModalLabel">
            <div class="modal-dialog" role="document">
                <div class="modal-content">
                <div class="modal-header">
                    <h4 class="modal-title" id="myModalLabel"></h4>
                </div>
                <div class="modal-body confirm-dialog-body">
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-default" data-dismiss="modal" id="cancel-action">Cancel</button>
                    <button type="button" class="btn btn-success" id="confirm-action">Confirm</button>
                </div>
                </div>
            </div>
            </div>`);

    $(".open-confirm-modal").on('click', (e) => {
        e.preventDefault();

        target = e.target;
        var Id = $(target).data('id');
        var controller = $(target).data('controller');
        var action = $(target).data('action');
        var bodyMessage = $(target).data('body-message');
        redirectUrl = $(target).data('redirect-url');
        var title = $(target).data('title');

        url = `/${controller}/${action}?Id=${Id}`;
        $(".confirm-dialog-body").html(bodyMessage);
        $(".modal-title").text(title);
        $("#confirmModal").modal('show');
    });

    $("#cancel-action").on('click', () => $("#confirmModal").modal('hide'))

    $("#confirm-action").on('click', () => {
        $.get(url)
            .done(() => {
                if (redirectUrl) {
                    window.location.href = redirectUrl;
                }
                else {
                    location.reload();
                }
            })
            .always(() => {
                $("#confirmModal").modal('hide');
            });
    });
}()));