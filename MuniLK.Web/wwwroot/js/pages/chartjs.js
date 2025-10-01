/**
 * Theme: Velonic - Responsive Bootstrap 5 Admin Dashboard
 * Author: Techzaa
 * Module/App: Chartjs
 */

window.loadChartJSCharts = function () {

  !function ($) {
    "use strict";

    var ChartJS = function () {
      this.$body = $("body");
      this.charts = [];
      this.defaultColors = ["#3bc0c3", "#4489e4", "#d03f3f", "#716cb0"];
    };

    ChartJS.prototype.boundariesExample = function () {
      var chartElement = document.getElementById('boundaries-example');
      var dataColors = chartElement.getAttribute('data-colors');
      var colors = dataColors ? dataColors.split(",") : this.defaultColors;
      var ctx = chartElement.getContext('2d');
      var chart = new Chart(ctx, {
        type: 'line', data: {
          labels: ['Jan', 'Feb', 'March', 'April', 'May', 'June'], datasets: [{
            label: 'Fully Rounded',
            data: [12.5, -19.4, 14.3, -15.0, 10.8, -10.5],
            borderColor: colors[0],
            backgroundColor: hexToRGB(colors[0], .3),
            fill: false
          },]
        }, options: {
          responsive: true, maintainAspectRatio: false,

          plugins: {
            legend: {
              display: false,

              position: 'top',
            },

          }, scales: {
            x: {
              grid: {
                display: false
              }
            }, y: {
              grid: {
                display: false
              }
            },
          }
        },
      });

      this.charts.push(chart);
    }

    ChartJS.prototype.datasetExample = function () {
      var chartElement = document.getElementById('dataset-example');
      var dataColors = chartElement.getAttribute('data-colors');
      var colors = dataColors ? dataColors.split(",") : this.defaultColors
      var ctx = chartElement.getContext('2d');
      var chart = new Chart(ctx, {
        type: 'line', data: {
          labels: ['Jan', 'Feb', 'March', 'April', 'May', 'June'], datasets: [{
            label: 'D0',
            data: [10, 20, 15, 35, 38, 24],
            borderColor: colors[0],
            hidden: true,
            backgroundColor: hexToRGB(colors[0], 0.3),
          }, {
            label: 'D1',
            data: [12, 18, 18, 33, 41, 20],
            borderColor: colors[1],
            fill: '-1',
            backgroundColor: hexToRGB(colors[1], 0.3),
          }, {
            label: 'D2',
            data: [5, 25, 20, 25, 28, 14],
            borderColor: colors[2],
            fill: 1,
            backgroundColor: hexToRGB(colors[2], 0.3),
          }, {
            label: 'D3',
            data: [12, 45, 15, 35, 38, 24],
            borderColor: colors[3],
            fill: '-1',
            backgroundColor: hexToRGB(colors[3], 0.3),
          }, {
            label: 'D4',
            data: [24, 38, 35, 15, 20, 10],
            borderColor: colors[4],
            fill: 8,
            backgroundColor: hexToRGB(colors[4], 0.3),
          }]
        }, options: {
          responsive: true, maintainAspectRatio: false,

          plugins: {

            filler: {
              propagate: false
            },

          }, interaction: {
            intersect: false,
          }, scales: {
            x: {
              grid: {
                display: false
              }
            }, y: {
              stacked: true, grid: {
                display: false
              }
            },
          }
        },
      });

      this.charts.push(chart)
    }

    ChartJS.prototype.borderRadiusExample = function () {
      var chartElement = document.getElementById('border-radius-example');
      var dataColors = chartElement.getAttribute('data-colors');
      var colors = dataColors ? dataColors.split(",") : this.defaultColors;
      var ctx = chartElement.getContext('2d');
      var chart = new Chart(ctx, {
        type: 'bar', data: {
          labels: ['Jan', 'Feb', 'March', 'April', 'May', 'June'], datasets: [{
            label: 'Fully Rounded',
            data: [12, -19, 14, -15, 12, -14],
            borderColor: colors[0],
            backgroundColor: hexToRGB(colors[0], .3),
            borderWidth: 2,
            borderRadius: Number.MAX_VALUE,
            borderSkipped: false,
          }, {
            label: 'Small Radius',
            data: [-10, 19, -15, -8, 12, -7],
            backgroundColor: hexToRGB(colors[1], .3),
            borderColor: colors[1],
            borderWidth: 2,
            borderRadius: Number.MAX_VALUE,
            borderSkipped: false,
          }]
        }, options: {
          responsive: true, maintainAspectRatio: false,

          plugins: {
            legend: {
              display: false,

              position: 'top',
            },

          }, scales: {
            x: {
              grid: {
                display: false
              }
            }, y: {
              grid: {
                display: false
              }
            },
          }
        },
      });

      this.charts.push(chart);
    }

    ChartJS.prototype.floatingBarExample = function () {
      var chartElement = document.getElementById('floating-example');
      var dataColors = chartElement.getAttribute('data-colors');
      var colors = dataColors ? dataColors.split(",") : this.defaultColors
      var ctx = chartElement.getContext('2d');
      var chart = new Chart(ctx, {
        type: 'bar', data: {
          labels: ['Jan', 'Feb', 'March', 'April', 'May', 'June'], datasets: [{
            label: 'Fully Rounded', data: [12, -19, 14, -15, 12, -14], backgroundColor: colors[0],
          }, {
            label: 'Small Radius', data: [-10, 19, -15, -8, 12, -7], backgroundColor: colors[1],
          }]
        }, options: {
          responsive: true, maintainAspectRatio: false,

          plugins: {
            legend: {
              display: false,

              position: 'top',
            },

          }, scales: {
            x: {
              grid: {
                display: false
              }
            }, y: {
              grid: {
                display: false
              }
            },
          }
        },
      });

      this.charts.push(chart)
    }

    ChartJS.prototype.interpolationExample = function () {
      var chartElement = document.getElementById('interpolation-example');
      var dataColors = chartElement.getAttribute('data-colors');
      var colors = dataColors ? dataColors.split(",") : this.defaultColors;
      var ctx = chartElement.getContext('2d');
      var datapoints = [0, 20, 20, 60, 60, 120, NaN, 180, 120, 125, 105, 110, 170];

      var chart = new Chart(ctx, {
        type: 'line', data: {
          labels: ['1', '2', '3', '4', '5', '6', '7', '8', '9', '10', '11', '12'], datasets: [{
            label: 'Fully Rounded',
            data: datapoints,
            borderColor: colors[0],
            fill: false,
            cubicInterpolationMode: 'monotone',
            tension: 0.4
          }, {
            label: 'Small Radius', data: datapoints, borderColor: colors[1], fill: false, tension: 0.4
          }, {
            label: 'Small Radius', data: datapoints, borderColor: colors[2], fill: false,
          },]
        }, options: {
          responsive: true, maintainAspectRatio: false, interaction: {
            intersect: false,
          }, plugins: {
            legend: {
              display: false,

              position: 'top',
            },

          }, scales: {
            x: {
              grid: {
                display: false
              }
            }, y: {
              grid: {
                display: false
              }, suggestedMin: -10, suggestedMax: 200
            },
          }
        },
      });

      this.charts.push(chart);
    }

    ChartJS.prototype.lineExample = function () {
      var chartElement = document.getElementById('line-example');
      var dataColors = chartElement.getAttribute('data-colors');
      var colors = dataColors ? dataColors.split(",") : this.defaultColors
      var ctx = chartElement.getContext('2d');
      var chart = new Chart(ctx, {
        type: 'line', data: {
          labels: ["Mon", "Tue", "Wed", "Thu", "Fri", "Sat", "Sun"], datasets: [{
            label: 'Fully Rounded',
            data: [32, 42, 42, 62, 52, 75, 62],
            borderColor: colors[0],
            fill: true,
            backgroundColor: hexToRGB(colors[0], 0.3),

          }, {
            label: 'Small Radius',
            data: [42, 58, 66, 93, 82, 105, 92],
            fill: true,
            backgroundColor: 'transparent',
            borderColor: colors[1],
            borderDash: [5, 5],
          }]
        }, options: {
          responsive: true, maintainAspectRatio: false,

          plugins: {
            legend: {
              display: false,

              position: 'top',
            },

          }, tension: 0.3, scales: {
            x: {
              grid: {
                display: false
              }
            }, y: {
              grid: {
                display: false
              }
            },
          }
        },
      });

      this.charts.push(chart)
    }

    ChartJS.prototype.bubbleExample = function () {
      var chartElement = document.getElementById('bubble-example');
      var dataColors = chartElement.getAttribute('data-colors');
      var colors = dataColors ? dataColors.split(",") : this.defaultColors;
      var ctx = chartElement.getContext('2d');
      var chart = new Chart(ctx, {
        type: 'bubble', data: {
          labels: ['Jan', 'Feb', 'March', 'April', 'May', 'June'], datasets: [{
            label: 'Fully Rounded',
            data: [{ x: 10, y: 20, r: 5 }, { x: 20, y: 10, r: 5 }, { x: 15, y: 15, r: 5 },],
            borderColor: colors[0],
            backgroundColor: hexToRGB(colors[0], .3),
            borderWidth: 2,
            borderSkipped: false,
          }, {
            label: 'Small Radius',
            data: [{ x: 12, y: 22 }, { x: 22, y: 20 }, { x: 5, y: 15 },],
            backgroundColor: hexToRGB(colors[1], .3),
            borderColor: colors[1],
            borderWidth: 2,
            borderSkipped: false,
          }]
        }, options: {
          responsive: true, maintainAspectRatio: false,

          plugins: {
            legend: {
              display: false,

              position: 'top',
            },

          }, scales: {
            x: {
              grid: {
                display: false
              }
            }, y: {
              grid: {
                display: false
              }
            },
          }
        },
      });

      this.charts.push(chart);
    }

    ChartJS.prototype.donutExample = function () {
      var chartElement = document.getElementById('donut-example');
      var dataColors = chartElement.getAttribute('data-colors');
      var colors = dataColors ? dataColors.split(",") : this.defaultColors
      var ctx = chartElement.getContext('2d');
      var chart = new Chart(ctx, {
        type: 'doughnut', data: {
          labels: ["Direct", "Affilliate", "Sponsored", "E-mail"], datasets: [{
            data: [200, 184, 96, 198], backgroundColor: colors, borderColor: "transparent", borderWidth: "3",
          }]
        }, options: {
          responsive: true, maintainAspectRatio: false, cutoutPercentage: 60, plugins: {
            legend: {
              display: false,

              position: 'top',
            },

          },
        },
      });

      this.charts.push(chart)
    }

    ChartJS.prototype.radarExample = function () {
      var chartElement = document.getElementById('radar-example');
      var dataColors = chartElement.getAttribute('data-colors');
      var colors = dataColors ? dataColors.split(",") : this.defaultColors
      var ctx = chartElement.getContext('2d');
      var chart = new Chart(ctx, {
        type: 'radar', data: {
          labels: ['Jan', 'Feb', 'March', 'April', "May", "June"], datasets: [{
            label: 'Dataset 1',
            data: [12, 29, 39, 22, 28, 34],
            borderColor: colors[0],
            backgroundColor: hexToRGB(colors[0], .3),

          }, {
            label: 'Dataset 2',
            data: [10, 19, 15, 28, 34, 39],
            borderColor: colors[1],
            backgroundColor: hexToRGB(colors[1], .3),

          },]
        }, options: {
          responsive: true, maintainAspectRatio: false, plugins: {
            legend: {
              display: false,
            },

          },
        },
      });
      this.charts.push(chart)
    }

    ChartJS.prototype.scatterExample = function () {
      var chartElement = document.getElementById('scatter-example');
      var dataColors = chartElement.getAttribute('data-colors');
      var colors = dataColors ? dataColors.split(",") : this.defaultColors
      var ctx = chartElement.getContext('2d');
      var chart = new Chart(ctx, {
        type: 'scatter', data: {
          labels: ['Jan', 'Feb', 'March', 'April', "May", "June", "July"], datasets: [{
            label: 'Dataset 1',
            data: [{ x: 10, y: 50, }, { x: 50, y: 10, }, { x: 15, y: 15, }, { x: 20, y: 45, }, { x: 25, y: 18, }, { x: 34, y: 38, },],
            borderColor: colors[0],
            backgroundColor: hexToRGB(colors[0], .3),

          }, {
            label: 'Dataset 2',
            data: [{ x: 15, y: 45, }, { x: 40, y: 20, }, { x: 30, y: 5, }, { x: 35, y: 25, }, { x: 18, y: 25, }, { x: 40, y: 8, },],
            borderColor: colors[1],
            backgroundColor: hexToRGB(colors[1], .3),

          }]
        }, options: {
          responsive: true, maintainAspectRatio: false, plugins: {
            legend: {
              display: false,
            },

          }, scales: {

            x: {
              grid: {
                display: false
              }
            }, y: {
              grid: {
                display: false
              }
            }
          }
        },
      });
      this.charts.push(chart)
    }

    ChartJS.prototype.barLineExample = function () {
      var chartElement = document.getElementById('bar-line-example');
      var dataColors = chartElement.getAttribute('data-colors');
      var colors = dataColors ? dataColors.split(",") : this.defaultColors
      var ctx = chartElement.getContext('2d');
      var chart = new Chart(ctx, {
        type: 'line', data: {
          labels: ['Jan', 'Feb', 'March', 'April', "May", "June", "July"], datasets: [{
            label: 'Dataset 1',
            data: [10, 20, 35, 18, 15, 25, 22],
            backgroundColor: colors[0],
            stack: 'combined',
            type: 'bar'
          }, {
            label: 'Dataset 2',
            data: [13, 23, 38, 22, 25, 30, 28],

            borderColor: colors[1],
            stack: 'combined'

          }]
        }, options: {
          responsive: true, maintainAspectRatio: false, plugins: {
            legend: {
              display: false,
            },

          }, scales: {

            x: {
              grid: {
                display: false
              }
            }, y: {
              stacked: true,
              grid: {
                display: false
              }
            }
          }
        },
      });
      this.charts.push(chart)
    }

    //initializing various components and plugins
    ChartJS.prototype.init = function () {
      var $this = this;
      Chart.defaults.font.family = '-apple-system,BlinkMacSystemFont,"Segoe UI",Roboto,Oxygen-Sans,Ubuntu,Cantarell,"Helvetica Neue",sans-serif';

      Chart.defaults.color = "#8391a2";
      Chart.defaults.scale.grid.color = "#8391a2";
      // init charts
      this.boundariesExample();
      this.datasetExample();
      this.borderRadiusExample();
      this.floatingBarExample();
      this.interpolationExample();
      this.lineExample();
      this.bubbleExample();
      this.donutExample();

      // enable resizing matter


      $(window).on('resizeEnd', function (e) {
        $.each($this.charts, function (index, chart) {
          try {
            chart.destroy();
          } catch (err) {
          }
        });

        $this.boundariesExample();
        $this.datasetExample();
        $this.borderRadiusExample();
        $this.floatingBarExample();
        $this.interpolationExample();
        $this.lineExample();
        $this.bubbleExample();
        $this.donutExample();
      });

      $(window).resize(function () {
        if (this.resizeTO) clearTimeout(this.resizeTO);
        this.resizeTO = setTimeout(function () {
          $(this).trigger('resizeEnd');
        }, 500);
      });
    };

    //init chart
    $.ChartJs = new ChartJS;
    $.ChartJs.Constructor = ChartJS
  }(window.jQuery),

    //initializing ChartJs
    function ($) {
      "use strict";
      $.ChartJs.init()
    }(window.jQuery);

  /* utility function */

  function hexToRGB(hex, alpha) {
    var r = parseInt(hex.slice(1, 3), 16), g = parseInt(hex.slice(3, 5), 16), b = parseInt(hex.slice(5, 7), 16);

    if (alpha) {
      return "rgba(" + r + ", " + g + ", " + b + ", " + alpha + ")";
    } else {
      return "rgb(" + r + ", " + g + ", " + b + ")";
    }
  }

}