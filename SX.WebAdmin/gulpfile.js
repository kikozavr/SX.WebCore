var promise = require('es6-promise'),
    gulp = require('gulp'),
    watch = require('gulp-watch'),
    del = require('del'),
    concat = require('gulp-concat'),
    uglify = require('gulp-uglify'),
    merge = require('merge-stream'),
    order = require('gulp-order'),
    less = require('gulp-less'),
    cleanCSS = require('gulp-clean-css'),
    autoprefixer = require('gulp-autoprefixer'),
    rename = require('gulp-rename');

//clear all files
function clear() {
    del([
        'content/dist/css/**/*.css',
        'content/dist/js/**/*.js',
        'content/dist/fonts/**/*'
    ]);
}

//create css files
function createCss() {
    var lessStream = gulp.src([
       'less/site.less',
       'less/sx-gv.less'
    ])
        .pipe(less())
        .pipe(cleanCSS({ compatibility: 'ie8' }))
        .pipe(autoprefixer('last 2 version', 'safari 5', 'ie 8', 'ie 9'))
        .pipe(concat('sitecss.css'));

    var cssStream = gulp.src([
        'bower_components/bootstrap/dist/css/bootstrap.min.css',
        'bower_components/font-awesome/css/font-awesome.min.css',
        'bower_components/metisMenu/dist/metisMenu.min.css'
    ])
        .pipe(concat('css.css'));

    var byOneLessStream = gulp.src([
       'less/login.less'
    ])
        .pipe(less())
        .pipe(cleanCSS({ compatibility: 'ie8' }))
        .pipe(autoprefixer('last 2 version', 'safari 5', 'ie 8', 'ie 9'))
        .pipe(rename({
            suffix: '.min'
        }))
        .pipe(gulp.dest('content/dist/css'));

    var mergedStream = merge(lessStream, cssStream)
        .pipe(order([
            'css.css',
            'sitecss.css'
        ]))
            .pipe(concat('site.min.css'))
            .pipe(gulp.dest('content/dist/css'));
}

//create fonts
function createFonts() {
    gulp.src([
        'bower_components/font-awesome/fonts/**/*'
    ])
        .pipe(gulp.dest('content/dist/fonts'));
}

//create js files
function createJs() {
    var js = gulp.src([
        'bower_components/jquery/dist/jquery.min.js',
        'bower_components/bootstrap/dist/js/bootstrap.min.js',
        'bower_components/metisMenu/dist/metisMenu.min.js',
    ])
        .pipe(concat('js.js'));

    var sitejs = gulp.src([
        'scripts/site.js'
    ])
        .pipe(uglify())
        .pipe(concat('sitejs.js'));

    var mergedStream = merge(js, sitejs)
        .pipe(order([
            'js.js',
            'sitejs.js'
        ]))
            .pipe(concat('site.min.js'))
            .pipe(gulp.dest('content/dist/js'));

    gulp.src([
        'bower_components/jquery-validation/dist/jquery.validate.min.js',
        'bower_components/jquery-ajax-unobtrusive/jquery.unobtrusive-ajax.min.js',
        'bower_components/jquery-validation-unobtrusive/jquery.validate.unobtrusive.min.js'
    ])
        .pipe(gulp.dest('content/dist/js'));
    
}

gulp.task('watch', function (cb) {
    watch([
        'less/**/*.less',
        'scripts/**/*.js'
    ], function () {
        clear();
        createCss();
        createFonts();
        createJs();
    });
});