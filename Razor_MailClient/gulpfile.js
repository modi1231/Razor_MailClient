/// <binding BeforeBuild='bootstrap_css, jquery' Clean='clean' />

//task runner explorer
//https://coursetro.com/posts/design/72/Installing-Bootstrap-4-Tutorial
var gulp = require('gulp');
var del = require('del');


gulp.task('clean', function ()
{
    del(["wwwroot/js/*.js", "wwwroot/css/*.css"]);/*, "wwwroot/lib/*.*"*/
});

gulp.task('jquery', function ()
{
    gulp.src(["node_modules/jquery/dist/jquery.js"])
        .pipe(gulp.dest("wwwroot/js"));

    gulp.src(["node_modules/bootstrap/dist/js/bootstrap.min.js"])
        .pipe(gulp.dest("wwwroot/js"));

    gulp.src(["node_modules/jquery-validation/dist/jquery.validate.js"])
        .pipe(gulp.dest("wwwroot/js"));

    gulp.src(["node_modules/jquery-validation-unobtrusive/dist/jquery.validate.unobtrusive.js"])
        .pipe(gulp.dest("wwwroot/js"));
});

gulp.task('bootstrap_css', function ()
{
    gulp.src(["node_modules/bootstrap/dist/css/bootstrap.css"])
        .pipe(gulp.dest("wwwroot/css"));
});
