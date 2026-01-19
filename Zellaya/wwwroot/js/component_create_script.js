document.addEventListener("DOMContentLoaded", function () {
    const typeInput = document.getElementById("type_component");

    typeInput.addEventListener("input", function () {
        let v = this.value.toLowerCase();
        v = v.replace(/[^a-zа-яё\s\-]/gi, "");
        v = v.replace(/\s+/g, " ");
        this.value = v;
    });
});