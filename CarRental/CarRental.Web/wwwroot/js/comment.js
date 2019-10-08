"use strict";

tinymce.init({ selector: '.html-editor' });
var connection = new signalR.HubConnectionBuilder()
    .withUrl("/vehiclemodelshub")
    .build();

var vehicleModelId = '@Html.Raw(Model.VehicleModel.Id)';
console.log(vehicleModelId);

connection.start().then(function () {
    connection.send('JoinVehicleModelPage', vehicleModelId).then(function () { });
        });

connection.on('CommentPosted', function (htmlString) {
    $("#comments").prepend(htmlString);
});

$('#new-comment-form').submit(function (e) {
    e.preventDefault();
    console.log(e);
    connection.send('PostComment', vehicleModelId, tinymce.activeEditor.getContent()).then(function () { });
        });

connection.on('CommentDeleted', function (commentId) {
    $("#comment-" + commentId).remove();
});
$('.delete-comment').click(function (e) {
    var commentId = e.target.attributes['data-comment-id'].value;
    console.log(e);
    connection.send('DeleteComment', commentId).then();
});