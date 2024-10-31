const sidebar = document.querySelector(".sidebar"); // Assuming your sidebar has a class named "sidebar"
const sidebarToggler = document.querySelector(".sidebar-toggler");

sidebarToggler.addEventListener("click", () => {
    sidebar.classList.toggle("collapsed"); // Toggles a class that shows/hides the sidebar
});
