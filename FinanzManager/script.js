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
function loadPersonsFromDB() {
    console.log("DB-Aufruf: Lade alle Personen...");
    const container = document.getElementById("Schulden-Container");
    
    // Platzhalter für das spätere Fetching
    container.innerHTML = `
        <div class="app" onclick="loadPersonDetails(1)">
            <h3>Max Mustermann</h3>
            <div class="amount">45,00 €</div>
            <div class="details">Zuletzt geändert: Gestern</div>
        </div>
    `;
}

// Ebene 3: Läuft beim Klick auf eine Personenkachel
function loadPersonDetails(personId) {
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
        <div class="app-card-dynamic">
            <h3>Fitnessstudio</h3>
            <div class="amount">29,90 €</div>
            <div class="details">Monatlich (1. des Monats)</div>
        </div>
    `;
}