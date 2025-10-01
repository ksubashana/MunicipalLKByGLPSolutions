
window.loadSweetAlerts = function () {

  //Basic
  document.getElementById("sweetalert-basic").addEventListener("click", function () {
    Swal.fire({
      title: 'Any fool can use a computer',
      confirmButtonClass: 'btn btn-primary w-xs mt-2',
      buttonsStyling: false,
      showCloseButton: false
    })
  });

  //A title with a text under
  document.getElementById("sweetalert-title").addEventListener("click", function () {
    Swal.fire({
      title: "The Internet?",
      text: 'That thing is still around?',
      icon: 'question',
      confirmButtonClass: 'btn btn-primary w-xs mt-2',
      buttonsStyling: false,
      showCloseButton: false
    })
  });

  //Success Message
  document.getElementById("sweetalert-success").addEventListener("click", function () {
    Swal.fire({
      title: 'Good job!',
      text: 'You clicked the button!',
      icon: 'success',
      showCancelButton: true,
      confirmButtonClass: 'btn btn-primary w-xs me-2 mt-2',
      cancelButtonClass: 'btn btn-danger w-xs mt-2',
      buttonsStyling: false,
      showCloseButton: false
    })
  });

  //error Message
  document.getElementById("sweetalert-error").addEventListener("click", function () {
    Swal.fire({
      title: 'Oops...',
      text: 'Something went wrong!',
      icon: 'error',
      confirmButtonClass: 'btn btn-primary w-xs mt-2',
      buttonsStyling: false,
      footer: '<a href="">Why do I have this issue?</a>',
      showCloseButton: false
    })
  });


  //info Message
  document.getElementById("sweetalert-info").addEventListener("click", function () {
    Swal.fire({
      title: 'Oops...',
      text: 'Something went wrong!',
      icon: 'info',
      confirmButtonClass: 'btn btn-primary w-xs mt-2',
      buttonsStyling: false,
      footer: '<a href="">Why do I have this issue?</a>',
      showCloseButton: false
    })
  });

  //Warning Message
  document.getElementById("sweetalert-warning").addEventListener("click", function () {
    Swal.fire({
      title: 'Oops...',
      text: 'Something went wrong!',
      icon: 'warning',
      confirmButtonClass: 'btn btn-primary w-xs mt-2',
      buttonsStyling: false,
      footer: '<a href="">Why do I have this issue?</a>',
      showCloseButton: false
    })
  });

  // long content
  document.getElementById("sweetalert-longcontent").addEventListener("click", function () {
    Swal.fire({
      imageUrl: 'https://placeholder.pics/svg/300x1500',
      imageHeight: 1500,
      imageAlt: 'A tall image',
      confirmButtonClass: 'btn btn-primary w-xs mt-2',
      buttonsStyling: false,
      showCloseButton: false
    })
  });


  //Parameter
  document.getElementById("sweetalert-params").addEventListener("click", function () {
    Swal.fire({
      title: 'Are you sure?',
      text: "You won't be able to revert this!",
      icon: 'warning',
      showCancelButton: true,
      confirmButtonText: 'Yes, delete it!',
      cancelButtonText: 'No, cancel!',
      confirmButtonClass: 'btn btn-primary w-xs me-2 mt-2',
      cancelButtonClass: 'btn btn-danger w-xs mt-2',
      buttonsStyling: false,
      showCloseButton: false
    }).then(function (result) {
      if (result.value) {
        Swal.fire({
          title: 'Deleted!',
          text: 'Your file has been deleted.',
          icon: 'success',
          confirmButtonClass: 'btn btn-primary w-xs mt-2',
          buttonsStyling: false
        })
      } else if (
        // Read more about handling dismissals
        result.dismiss === Swal.DismissReason.cancel
      ) {
        Swal.fire({
          title: 'Cancelled',
          text: 'Your imaginary file is safe :)',
          icon: 'error',
          confirmButtonClass: 'btn btn-primary mt-2',
          buttonsStyling: false
        })
      }
    });
  });

}