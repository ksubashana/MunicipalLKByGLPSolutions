const { series, src, dest, parallel, watch } = require("gulp");
const autoprefixer = require("gulp-autoprefixer");
const CleanCSS = require("gulp-clean-css");
const concat = require("gulp-concat");
const rename = require("gulp-rename");
const sourcemaps = require("gulp-sourcemaps");
const sass = require("gulp-sass")(require("sass"));
const uglify = require("gulp-uglify");
const npmdist = require("gulp-npm-dist");
const rtlcss = require("gulp-rtlcss");

const paths = {
  baseDistAssets: "wwwroot/",         // build assets directory
  baseSrcAssets: "wwwroot/",          // source assets directory

  plugin: {
    styles: [
      "./node_modules/dropzone/dist/dropzone.css",
      "./node_modules/flatpickr/dist/flatpickr.css",
      "./node_modules/swiper/swiper-bundle.min.css",
      "./node_modules/sweetalert2/dist/sweetalert2.min.css",
      "./node_modules/choices.js/public/assets/styles/choices.min.css",
      "./node_modules/nouislider/dist/nouislider.min.css",
      "./node_modules/multi.js/dist/multi.min.css",
      "./node_modules/quill/dist/quill.core.css",
      "./node_modules/quill/dist/quill.bubble.css",
      "./node_modules/quill/dist/quill.snow.css",
      "./node_modules/gridjs/dist/theme/mermaid.min.css"
    ],

    scripts: [
      "./node_modules/jquery/dist/jquery.min.js",
      "./node_modules/bootstrap/dist/js/bootstrap.bundle.js",
      "./node_modules/simplebar/dist/simplebar.min.js",
      "./node_modules/apexcharts/dist/apexcharts.min.js",
      "./node_modules/prismjs/prism.js",
      "./node_modules/prismjs/plugins/normalize-whitespace/prism-normalize-whitespace.js",
      "./node_modules/toastify-js/src/toastify.js",
      "./node_modules/vanilla-wizard/dist/js/wizard.min.js",
      "./node_modules/clipboard/dist/clipboard.min.js",
      "./node_modules/moment/moment.js",
      "./node_modules/dropzone/dist/dropzone-min.js",
      "./node_modules/flatpickr/dist/flatpickr.js",
      "./node_modules/swiper/swiper-bundle.min.js",
      "./node_modules/sweetalert2/dist/sweetalert2.min.js",
      "./node_modules/inputmask/dist/inputmask.min.js",
      "./node_modules/choices.js/public/assets/scripts/choices.min.js",
      "./node_modules/nouislider/dist/nouislider.min.js",
      "./node_modules/multi.js/dist/multi.min.js",
      "./node_modules/quill/dist/quill.core.js",
      "./node_modules/quill/dist/quill.js",
      "./node_modules/wnumb/wNumb.min.js",
      "./node_modules/iconify-icon/dist/iconify-icon.min.js",
      "./node_modules/dayjs/dayjs.min.js",
      "./node_modules/gridjs/dist/gridjs.production.min.js",
      "./node_modules/gmaps/gmaps.min.js",
      "./node_modules/jsvectormap/dist/jsvectormap.min.js",
      "./node_modules/jsvectormap/dist/maps/world-merc.js",
      "./node_modules/jsvectormap/dist/maps/world.js",
      "./node_modules/chart.js/dist/chart.min.js",
      "./node_modules/jquery-sparkline/jquery.sparkline.min.js"
    ]
  },

};

const plugins = function () {

  const outcss = paths.baseDistAssets + "css/";

  src(paths.plugin.styles)
    .pipe(concat("vendor.min.css"))
    .pipe(CleanCSS())
    .pipe(dest(outcss));

  const outjs = paths.baseDistAssets + "js/";

  src(paths.plugin.scripts)
    .pipe(concat("vendor.js"))
    .pipe(dest(outjs))
    .pipe(uglify())
    .pipe(rename({ suffix: ".min" }))
    .pipe(dest(outjs));

  const out = paths.baseDistAssets + "vendor/";
  return src(npmdist(), { base: "./node_modules" })
    .pipe(rename(function (path) {
      path.dirname = path.dirname.replace(/\/dist/, '').replace(/\\dist/, '');
    }))
    .pipe(dest(out));
};


const scss = function () {
  const out = paths.baseDistAssets + "css/";

  src(paths.baseSrcAssets + "scss/app.scss")
    .pipe(sourcemaps.init())
    .pipe(sass.sync().on('error', sass.logError)) // scss to css
    .pipe(
      autoprefixer({
        overrideBrowserslist: ["last 2 versions"],
      })
    )
    .pipe(dest(out))
    .pipe(CleanCSS())
    .pipe(rename({ suffix: ".min" }))
    .pipe(sourcemaps.write("./"))
    .pipe(dest(out));

  // generate rtl
  return src(paths.baseSrcAssets + "scss/app.scss")
    .pipe(sourcemaps.init())
    .pipe(sass.sync().on('error', sass.logError)) // scss to css
    .pipe(
      autoprefixer({
        overrideBrowserslist: ["last 2 versions"],
      })
    )
    .pipe(rtlcss())
    .pipe(rename({ suffix: "-rtl" }))
    .pipe(dest(out))
    .pipe(CleanCSS())
    .pipe(rename({ suffix: ".min" }))
    .pipe(sourcemaps.write("./"))
    .pipe(dest(out));
};

const icons = function () {
  const out = paths.baseDistAssets + "css/";
  return src(paths.baseSrcAssets + "scss/icons.scss")
    .pipe(sourcemaps.init())
    .pipe(sass.sync().on('error', sass.logError)) // scss to css
    .pipe(
      autoprefixer({
        overrideBrowserslist: ["last 2 versions"],
      })
    )
    .pipe(dest(out))
    .pipe(CleanCSS())
    .pipe(rename({ suffix: ".min" }))
    .pipe(sourcemaps.write("./"))
    .pipe(dest(out));
};

// File Watch Task
function watchFiles() {
  watch([paths.baseSrcAssets + "scss/icons/*.scss", paths.baseSrcAssets + "scss/icons.scss"], series(icons));
  watch([paths.baseSrcAssets + "scss/**/*.scss", "!" + paths.baseSrcAssets + "scss/icons.scss"], series(scss));
}

// Production Tasks
exports.default = series(
  parallel(plugins, scss, icons),
  parallel(watchFiles)
);

// Build Tasks
exports.build = series(
  plugins,
  parallel(scss, icons)
);
