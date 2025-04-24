//document.addEventListener("DOMContentLoaded", function () {
//    console.log("JavaScript Loaded!");

//    const sidebar = document.getElementById("sidebar");
//    const menuBtn = document.getElementById("menuBtn");

//    if (!menuBtn || !sidebar) {
//        console.error("Sidebar or Menu Button not found!");
//        return;
//    }

//    menuBtn.addEventListener("click", function () {
//        console.log("Button Clicked!");
//        sidebar.classList.toggle("active");
//    });
//});

document.addEventListener("DOMContentLoaded", function () {
    console.log("JavaScript Loaded!");

    const sidebar = document.getElementById("sidebar");
    const menuBtn = document.getElementById("menuBtn");
    const dropdowns = document.querySelectorAll(".dropdown-toggle");

    if (menuBtn && sidebar) {
        menuBtn.addEventListener("click", function () {
            console.log("Button Clicked!");
            sidebar.classList.toggle("active");
        });
    } else {
        console.error("Error: Sidebar or Button not found!");
    }

    dropdowns.forEach(dropdown => {
        dropdown.addEventListener("click", function (event) {
            event.preventDefault(); 
            this.parentElement.classList.toggle("active");
        });
    });
});
