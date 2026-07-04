let currentPersonId = null; // Globale Variable, um die aktuelle Person zu speichern

/* ==========================================================================
   1. NAVIGATION (ANSICHTEN UMSCHALTEN)
   ========================================================================== */

// Hilfsfunktion: Blendet alle Sektionen aus
function hideAllSections() {
    document.getElementById("SchuldenSection").classList.add("hidden");
    document.getElementById("SchuldenDetailSection").classList.add("hidden");
    document.getElementById("FinanzSection").classList.add("hidden");
    
    // Falls das Hauptmenü (Ebene 1) in einem eigenen Container liegt, hier auch steuern:
    // Im aktuellen HTML liegt Ebene 1 direkt im body. Um sie auszublenden, packen wir sie optional in eine Section.
    // Für den Moment steuern wir die Sichtbarkeit der Dashboard-Kacheln direkt über den app-container.
    const mainDashboard = document.querySelector("body > .app-container:not([id])");
    const mainSubtitle = document.querySelector(".subtitle");
    
    if (mainDashboard) mainDashboard.classList.add("hidden");
    if (mainSubtitle) mainSubtitle.classList.add("hidden");
}

// Ebene 1: Zurück zum Dashboard
function openDashboard() {
    hideAllSections();
    
    // Dashboard-Elemente wieder einblenden
    const mainDashboard = document.querySelector("body > .app-container:not([id])");
    const mainSubtitle = document.querySelector(".subtitle");
    
    if (mainDashboard) mainDashboard.classList.remove("hidden");
    if (mainSubtitle) mainSubtitle.classList.remove("hidden");
}

// Ebene 2a: Schulden-Personenübersicht öffnen
function openDebts() {
    hideAllSections();
    document.getElementById("SchuldenSection").classList.remove("hidden");
    
    // Hier triggern wir direkt das Laden der Personen aus der DB
    loadPersonsFromDB();
}

// Ebene 2b: Eigene Finanzen / Raten öffnen
function openFinances() {
    hideAllSections();
    document.getElementById("FinanzSection").classList.remove("hidden");
    
    // Hier triggern wir das Laden der Verträge/Raten aus der DB
    loadFinancesFromDB();
}


/* ==========================================================================
   2. DYNAMISCHES LADEN & DB-SCHNITTSTELLEN (PLATZHALTER)
   Genau nach deiner Logik-Skizze aufgebaut!
   ========================================================================== */

// Läuft beim Klick auf "Schuldenbuch"
async function loadPersonsFromDB() 
{
    console.log("DB-Aufruf: Lade alle Personen...");
    const container = document.getElementById("Schulden-Container");

    container.innerHTML = 'div class="loading">Lade Personen...</div>'; // Ladehinweis anzeigen

    try {
        // Hier würdest du normalerweise die Daten von der API abrufen
        const response = await fetch('/api/Schuldenbuch/Person');
        
        if (!response.ok) {
            throw new Error(`Server-Fehler: ${response.status} ${response.statusText}`);
        }

        const persons = await response.json();

        if (persons.length === 0) {
            container.innerHTML = '<div class="empty-state">Keine Personen gefunden.</div>';
            return;
        }

        container.innerHTML = ''; // Ladehinweis entfernen

        // Dynamisches Erstellen der Personenkacheln
        persons.forEach(person => {
            const personCard = `
        <div class="app" onclick="loadPersonDetails(${person.id})">
            <h3>${escapeHtml(person.name)}</h3>
            <div class="amount">${formatCurrency(displayDebt)}</div>
            <div class="details">${escapeHtml(person.street)}, ${escapeHtml(person.zipCode)} ${escapeHtml(person.city)}</div>
        </div>
    `;

            container.insertAdjacentHTML('beforeend', personCard);
        });
}

// Ebene 3: Läuft beim Klick auf eine Personenkachel
function loadPersonDetails(personId) {

    currentPersonId = personId; // Speichern der aktuellen Person-ID

    hideAllSections();
    document.getElementById("SchuldenDetailSection").classList.remove("hidden");
    
    // Namen der Person dynamisch setzen
    document.getElementById("CurrentPersonName").innerText = `Schulden von Person #${personId}`;
    
    console.log(`DB-Aufruf: Lade spezifische Schulden für Person-ID: ${personId}...`);
    const container = document.getElementById("Personen-Schulden-Container");
    
    // Platzhalter für die konkreten Schuldposten dieser Person
    container.innerHTML = `
        <div class="app">
            <h3>Kasten Bier</h3>
            <div class="amount">15,50 €</div>
            <div class="details">Ausgeliehen am: 12.06.2026</div>
        </div>
    `;
}

// Läuft beim Klick auf "Finanzen"
function loadFinancesFromDB() {
    console.log("DB-Aufruf: Lade eigene Finanzen & Raten...");
    const container = document.getElementById("Finanzen-Container");
    
    container.innerHTML = `
        <div class="app">
            <h3>Fitnessstudio</h3>
            <div class="amount">29,90 €</div>
            <div class="details">Monatlich (1. des Monats)</div>
        </div>
    `;
}

//#region MODAL-FUNKTIONEN

function openModal(modalId) {
    document.getElementById(modalId).classList.remove("hidden");
}


function closeModal(modalId) {
    document.getElementById(modalId).classList.add("hidden");
}

//#endregion

//#region PERSONEN-HINZUFÜGEN
async function savePerson(event) {
    event.preventDefault(); // Verhindert das automatische Absenden des Formulars
    
    // Die Werte aus den Eingabefeldern abrufen
    const personData = {
        name: document.getElementById("person-name").value,
        street: document.getElementById("street").value,
        zipcode: document.getElementById("ZipCode").value,
        city: document.getElementById("city").value
    };
    
    try {
        // Senden der Daten an die API
        const response = await fetch('/api/Schuldenbuch/Person', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(personData)
        });

        if (response.ok) {
            console.log('Person erfolgreich gespeichert!');
            closeModal('AddPersonModal'); // Schließt das Modal nach dem Speichern
            loadPersonsFromDB(); // Aktualisiert die Liste der Personen
            
            closeAddPersonModal(); // Schließt das Modal nach dem Speichern
            document.getElementById("add-person-form").reset(); // Setzt das Formular zurück
        } else{
            console.error('Fehler beim Speichern der Person:', response.statusText);
            alert('Fehler beim Speichern der Person. Bitte versuchen Sie es erneut.');
        }
    } catch (error) {
        console.error('Fehler beim Speichern der Person:', error);
    }

}
//#endregion

//#region SCHULD-HINZUFÜGEN

async function saveEntry(event) {
    event.preventDefault(); // Verhindert das automatische Absenden des Formulars

    if (currentPersonId === null) {
        console.error('Keine Person ausgewählt. Bitte wählen Sie eine Person aus, bevor Sie eine Schuld hinzufügen.');
        alert('Keine Person ausgewählt. Bitte wählen Sie eine Person aus, bevor Sie eine Schuld hinzufügen.');
        return;
    }
    
    // Die Werte aus den Eingabefeldern abrufen
    const entryData = {
        
        personId: currentPersonId, // Verwende die globale Variable
        amount: document.getElementById("entry-amount").value,
        description: document.getElementById("entry-description").value
    };

    try {
        // Senden der Daten an die API
        const response = await fetch('/api/Schuldenbuch/Debt', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(entryData)
        });

        if (response.ok) {
            console.log('Schuld erfolgreich gespeichert!');
            closeModal('AddEntryModal'); // Schließt das Modal nach dem Speichern
            document.getElementById("add-entry-form").reset(); // Setzt das Formular zurück
            loadPersonDetails(currentPersonId); // Aktualisiert die Liste der Schulden für die aktuelle Person
        } else {
            console.error('Fehler beim Speichern der Schuld:', response.statusText);
            alert('Fehler beim Speichern der Schuld. Bitte versuchen Sie es erneut.');
        }
    } catch (error) {
        console.error('Fehler beim Speichern der Schuld:', error);
    }
}

//#endregion
