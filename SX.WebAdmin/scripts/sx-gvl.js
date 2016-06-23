(function ($) {
    $.fn.sx_gvl = function () {
        var width = (window.innerWidth > 0) ? window.innerWidth : screen.width;

        this.each(function () {
            var $this = $(this);

            $this.on('click', '.input-group > input[type="text"]', function (event) {
                $grid = $(this).closest('.sx-gvl');
                var isLoaded = $grid.attr('data-is-loaded');
                if (isLoaded == 'true') {
                    return false;
                };

                $content = $grid.find('.sx-gvl__content');
                var ajaxUrl = $grid.data('ajax-url');
                getGridLookupData(ajaxUrl, $content);
            });

            $this.on('click', '.input-group-btn button', function () {
                $(this).parent('span').prev('input[type="text"]').trigger('click');
            });
        });
    };
})(jQuery);

function getGridLookupData(ajaxUrl, contet) {
    $content = $(contet);

    $.ajax({
        method: 'post',
        url: ajaxUrl,
        success: function (data) {
            $content.closest('.sx-gvl').attr('data-is-loaded', 'true');
            $content.html(data);
            $content.next('.sx-gvl__bottom-panel').show();
            $content.sx_gv();
            $content.find('[data-toggle="tooltip"]').tooltip();
        }
    });
}