
    // Récupérer la référence de l'élément d'entrée de texte
    const searchInput = document.getElementById("searchInput");

    // Écouter l'événement de saisie utilisateur
    searchInput.addEventListener("input", function() {
        const searchText = searchInput.value.trim().toLowerCase();
        const tableRows = document.querySelectorAll("tbody tr");

        // Parcourir chaque ligne du tableau
        tableRows.forEach(function (row) {
            const ticketID = row.querySelector("td:nth-child(1)").textContent.toLowerCase();
            const ticketUser = row.querySelector("td:nth-child(2)").textContent.toLowerCase();
            const ticketTitle = row.querySelector("td:nth-child(3)").textContent.toLowerCase();
            const ticketDescription = row.querySelector("td:nth-child(4)").textContent.toLowerCase();
            const ticketDateCrea = row.querySelector("td:nth-child(5)").textContent.toLowerCase();
            const ticketPanne = row.querySelector("td:nth-child(7)").textContent.toLowerCase();

            // Cacher ou afficher la ligne en fonction de la correspondance avec le texte recherché
            if (ticketTitle.includes(searchText) || ticketDescription.includes(searchText) || ticketID.includes(searchText) || ticketUser.includes(searchText) || ticketDateCrea.includes(searchText) || ticketPanne.includes(searchText) ){
                row.style.display = "table-row";
            } else {
                row.style.display = "none";
            }
        });
    });

