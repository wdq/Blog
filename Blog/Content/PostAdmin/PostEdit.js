var ViewModel = function (data) {
    var self = this;

    ko.mapping.fromJS(data, null, self);

    self.serializeForm = function (form) {
        var array = form.serializeArray();
        var json = {};

        $.each(array, function () {
            json[this.name] = this.value || '';
        });

        return json;
    };

    self.save = function () {
        self.Body = CKEDITOR.instances.Body.getData();
        window.viewModel.Timestamp($('#datetimepicker4').val());
        $.ajax({
            url: "EditPost",
            type: "POST",
            data: ko.toJSON(self),
            contentType: "application/json",
            success: function (data) {
                if (data.length == 36) {
                    console.log(data);
                    window.location.href = "Edit?id=" + data;
                } else {
                    alert("Error saving post.");
                }
            }
        });
    }
}

$(function () {
    window.viewModel = new ViewModel(initialModel);
    ko.applyBindings(window.viewModel);
})

$(document).ready(function () {
    CKEDITOR.replace('Body');
});