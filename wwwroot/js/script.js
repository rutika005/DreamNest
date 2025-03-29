document.addEventListener("DOMContentLoaded", function () {
    const sidebar = document.getElementById("sidebar");
    const menuBtn = document.getElementById("menuBtn");

    menuBtn.addEventListener("click", function () {
        sidebar.classList.toggle("active");
    });
});