(function ($) {
    $.fn.sx_gv = function () {
        var width = (window.innerWidth > 0) ? window.innerWidth : screen.width;

        this.each(function () {
            var $this = $(this).closest('div');

            $this.on('click', '.sx-gv__pager a', function () {
                $a = $(this);
                $grid = $a.closest('.sx-gv');
                var isActive = $a.closest('li').hasClass('active');
                if (!isActive) {
                    var page = $a.data('page');

                    $order = $grid.find('.sx-gv__sort-arow');
                    if ($order.length == 1) {
                        var filelName = $order.closest('th').data('field-name');
                        var dir = $order.data('sort-direction');
                        var order = { FieldName: filelName, Direction: dir };
                        getGridViewData($grid, page, order);
                    }
                    else
                        getGridViewData($grid, page);
                }
            });

            $this.on('keypress', '.sx-gv__filter-row input', function (event) {
                if (event.which != 13) return;

                $input = $(this);
                $grid = $input.closest('.sx-gv');
                var page = $grid.find('.sx-gv__pager li.active a').data('page');

                getGridViewData($grid, page);
            });

            $this.on('click', '.sx-gv__clear-btn', function () {
                $a = $(this);
                $grid = $a.closest('.sx-gv');
                $grid.find('.sx-gv__filter-row input').val(null);
                getGridViewData($grid, 1);
            });

            $this.on('click', 'th', function () {
                $grid = $(this).closest('.sx-gv');
                var page = $grid.find('.sx-gv__pager li.active a').data('page');

                $order = $(this).find('.sx-gv__sort-arow');
                var filelName = $(this).data('field-name');

                var direction = 'Asc';
                if ($order.length == 1) {
                    var dir = $order.data('sort-direction');
                    if (dir == 'Asc')
                        direction = 'Desc';
                }
                var order = { FieldName: filelName, Direction: direction };

                getGridViewData($grid, page, order);
            });
        });
    };
})(jQuery);

function getGridViewData(grid, page, order) {
    $grid = $(grid);
    $pager = $grid.find('.sx-gv__pager');

    var ajaxUrl = $grid.data('ajax-url');

    var filterModel = {};
    $inputs = $grid.find('.sx-gv__filter-row input');
    $inputs.each(function () {
        $input = $(this);
        var value = $input.val();
        if (value != '') {
            var name = $input.attr('name');
            filterModel[name] = value;
        }
    });

    var data = {
        filterModel: filterModel,
        order: order,
        page: page
    };

    $.ajax({
        url: ajaxUrl,
        data: data,
        method: 'post',
        success: function (result) {
            $grid.html(result);
        },
        complete: function () {
            $grid.find('[data-toggle="tooltip"]').tooltip();
        }
    });
}