document.addEventListener("DOMContentLoaded", function () {
    console.log("JavaScript Loaded!"); // Debugging: Check if script is loaded

    const sidebar = document.getElementById("sidebar");
    const menuBtn = document.getElementById("menuBtn");

    if (menuBtn && sidebar) {  // Ensure Elements Exist
        menuBtn.addEventListener("click", function () {
            console.log("Button Clicked!"); // Debugging: Check if button is clicked
            sidebar.classList.toggle("active");
        });
    } else {
        console.error("Error: Sidebar or Button not found!");
    }
});
