(function () {
    // 100% защита от null
    const body = document.getElementById("balancesBody");
    const searchInput = document.getElementById("searchInput");
    const typeFilter = document.getElementById("typeFilter");
    const qtyFilter = document.getElementById("qtyFilter");
    const sortFilter = document.getElementById("sortFilter");
    const emptyBox = document.getElementById("emptyBox");

    if (!body || !searchInput || !typeFilter || !qtyFilter || !sortFilter || !emptyBox) {
        console.error("Balances: элементы не найдены. Проверь id в html.");
        return;
    }

    function rows() {
        return Array.from(body.querySelectorAll("tr.row"));
    }

    function fillTypeFilter() {
        const set = new Set();

        rows().forEach(r => {
            const t = (r.dataset.type || "").trim();
            if (t) set.add(t);
        });

        Array.from(set).sort().forEach(t => {
            const opt = document.createElement("option");
            opt.value = t;
            opt.textContent = t;
            typeFilter.appendChild(opt);
        });
    }

    function qtyGroup(qty) {
        if (qty <= 10) return "low";
        if (qty <= 50) return "mid";
        return "high";
    }

    function applyFilters() {
        const q = (searchInput.value || "").toLowerCase().trim();
        const type = typeFilter.value;
        const qtyMode = qtyFilter.value;

        let visible = 0;

        rows().forEach(r => {
            const name = (r.dataset.name || "").toLowerCase();
            const part = (r.dataset.part || "").toLowerCase();
            const rowType = (r.dataset.type || "");
            const qty = parseInt(r.dataset.qty || "0", 10);

            let ok = true;

            // search
            if (q && !(name.includes(q) || part.includes(q) || rowType.toLowerCase().includes(q))) ok = false;

            // type
            if (ok && type !== "all" && rowType !== type) ok = false;

            // qty
            if (ok && qtyMode !== "all") {
                const g = qtyGroup(qty);
                if (g !== qtyMode) ok = false;
            }

            r.style.display = ok ? "" : "none";
            if (ok) visible++;
        });

        emptyBox.classList.toggle("hidden", visible > 0);
    }

    function sortRows() {
        const mode = sortFilter.value;

        const list = rows();

        list.sort((a, b) => {
            const typeA = (a.dataset.type || "").toLowerCase();
            const typeB = (b.dataset.type || "").toLowerCase();
            const nameA = (a.dataset.name || "").toLowerCase();
            const nameB = (b.dataset.name || "").toLowerCase();
            const qtyA = parseInt(a.dataset.qty || "0", 10);
            const qtyB = parseInt(b.dataset.qty || "0", 10);

            if (mode === "type") return typeA.localeCompare(typeB) || nameA.localeCompare(nameB);
            if (mode === "name") return nameA.localeCompare(nameB);
            if (mode === "qty_desc") return qtyB - qtyA;
            if (mode === "qty_asc") return qtyA - qtyB;

            return 0;
        });

        list.forEach(r => body.appendChild(r));
        applyFilters();
    }

    // events
    searchInput.addEventListener("input", applyFilters);
    typeFilter.addEventListener("change", applyFilters);
    qtyFilter.addEventListener("change", applyFilters);
    sortFilter.addEventListener("change", sortRows);

    // init
    fillTypeFilter();
    sortRows();
})();
