/// <reference path="../bower_components/jquery/dist/jquery.min.js" />
(function ($) {
    $.fn.sx_gvl = function () {
        var width = (window.innerWidth > 0) ? window.innerWidth : screen.width;

        this.each(function () {
            var $this = $(this);
            var isSingleMode = $this.find('.sx-gvl').attr('data-is-single-mode');

            if (isSingleMode)
                $this.on('click', 'tbody tr:not(.sx-gv__filter-row)', function (e) {
                    $row = $(this);
                    $input = $row.closest('.sx-gvl').prev('input[type="hidden"]');
                    $textInput = $this.find('.sx-gvl__input');
                    $dropdown = $this.find('.sx-gvl__dropdown');
                    var id = $row.attr('data-row-id');
                    var text = $row.attr('data-row-text');
                    $input.val(id);
                    $textInput.val(text);
                    $dropdown.hide();
                });

            $this.on('click', '.sx-gvl__input', function () {
                $grid = $(this).closest('.sx-gvl');
                var isLoaded = $grid.attr('data-is-loaded');
                if (isLoaded == 'true') {
                    $dropdown = $grid.find('.sx-gvl__dropdown');
                    $dropdown.show();
                    return false;
                };

                $content = $grid.find('.sx-gvl__content');
                var ajaxUrl = $grid.attr('data-ajax-url');
                getGridLookupData(ajaxUrl, $content);
            });

            $this.on('click', '.sx-gvl__button', function () {
                $this.find('.sx-gvl__input').trigger('click');
            });
        });
    };
})(jQuery);

function getGridLookupData(ajaxUrl, contet) {
    $content = $(contet);
    $grid = $content.closest('.sx-gvl');
    $btn = $grid.find('.sx-gvl__button');

    $.ajax({
        method: 'post',
        url: ajaxUrl,
        beforeSend: function () {
            $btn.find('i').show();
        },
        success: function (data) {
            $grid.attr('data-is-loaded', 'true');
            $content.html(data);
            $content.sx_gv();
            $grid.find('.sx-gvl__dropdown').show();
            $content.find('[data-toggle="tooltip"]').tooltip();
            $btn.find('i').hide();
        }
    });
}