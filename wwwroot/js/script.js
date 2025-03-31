document.addEventListener("DOMContentLoaded", function () {
    console.log("JavaScript Loaded!");

    const sidebar = document.getElementById("sidebar");
    const menuBtn = document.getElementById("menuBtn");

    if (!menuBtn || !sidebar) {
        console.error("Sidebar or Menu Button not found!");
        return;
    }
    
    menuBtn.addEventListener("click", function () {
        console.log("Button Clicked!");
        sidebar.classList.toggle("active");
    });
});
