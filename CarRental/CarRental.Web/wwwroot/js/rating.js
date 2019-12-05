(function ($) {

    function Rating() {

        var $this = this;
        function initialize() {
            $(".star").click(function () {
                $(".star").removeClass('active');
                $(this).addClass('active');
                var starRating = $(this).data("value");
                $("#Rating").val(starRating);
            });
        }

        $this.init = function () {
            initialize();
        };
    }
    $(function () {
        var self = new Rating();
        self.init();
    });

}(jQuery));