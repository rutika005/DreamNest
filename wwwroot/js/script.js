document.addEventListener("DOMContentLoaded", function () {
    console.log("JavaScript Loaded!"); // Debugging: Check if script is loaded

    const sidebar = document.getElementById("sidebar");
    const menuBtn = document.getElementById("menuBtn");
    const mainContent = document.querySelector(".main-content");

    if (menuBtn && sidebar) {
        menuBtn.addEventListener("click", function () {
            console.log("Button Clicked!"); // Debugging: Check if button is clicked
            sidebar.classList.toggle("active");

            // Adjust main content when sidebar is open
            if (sidebar.classList.contains("active")) {
                sidebar.style.left = "0"; 
                mainContent.style.marginLeft = "250px"; 
            } else {
                sidebar.style.left = "-250px"; 
                mainContent.style.marginLeft = "0"; 
            }
        });
    } else {
        console.error("Error: Sidebar or Button not found!");
    }
});
