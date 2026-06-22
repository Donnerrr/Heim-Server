// Automatisch ermittelte Basis-URL deiner API (relativ für PWA-Einsatz)
const API_BASE = '/api/Schuldenbuch';

// DOM Elemente
const personsView = document.getElementById('persons-view');
const detailsView = document.getElementById('details-view');
const personsGrid = document.getElementById('persons-grid');
const debtsGrid = document.getElementById('debts-grid');
const selectedPersonName = document.getElementById('selected-person-name');
const btnBack = document.getElementById('btn-back');

let currentPersonId = null;

// Beim Start direkt die Personen laden
document.addEventListener('DOMContentLoaded', loadPersons);

// Zurück-Button Event
btnBack.addEventListener('click', () => {
    detailsView.classList.add('hidden');
    personsView.classList.remove('hidden');
    loadPersons(); // Aktualisieren
});

// 1. PERSONEN LADEN (Nutzt deinen PersonController [HttpGet])
async function loadPersons() {
    try {
        const response = await fetch(`${API_BASE}/Person`);
        if (!response.ok) throw new Error('Fehler beim Laden der Personen');
        
        const persons = await response.json();
        renderPersons(persons);
    } catch (error) {
        console.error(error);
        personsGrid.innerHTML = `<p class="status-text" style="color: #ff4d4d;">Verbindung zum Backend fehlgeschlagen.</p>`;
    }
}

// Personen-Kacheln rendern
function renderPersons(persons) {
    personsGrid.innerHTML = '';
    
    if (persons.length === 0) {
        personsGrid.innerHTML = '<p class="status-text">Noch keine Personen eingetragen.</p>';
        return;
    }

    persons.forEach(person => {
        const card = document.createElement('div');
        card.className = 'card';
        // Wir nehmen an, das Person-Objekt hat Id, Name und eine Gesamtsumme (TotalDebt)
        card.innerHTML = `
            <h3>${person.name || 'Unbekannt'}</h3>
            <div class="amount">${(person.totalDebt || 0).toFixed(2)} €</div>
        `;
        
        // Klick öffnet Detailseite
        card.addEventListener('click', () => openPersonDetails(person));
        personsGrid.appendChild(card);
    });
}

// 2. SCHULDEN DETAILS ANZEIGEN (Nutzt deinen PersonController [HttpGet("{id}")])
async function openPersonDetails(person) {
    currentPersonId = person.id;
    selectedPersonName.textContent = `Schulden von ${person.name}`;
    
    // Ansichten wechseln
    personsView.classList.add('hidden');
    detailsView.classList.remove('hidden');
    debtsGrid.innerHTML = 'Lade Schulden...';

    try {
        // Holt die spezifische Person inkl. ihrer Schulden-Liste aus deinem Backend
        const response = await fetch(`${API_BASE}/Person/${person.id}`);
        if (!response.ok) throw new Error('Fehler beim Laden der Details');
        
        const personData = await response.json();
        renderDebts(personData.debts || []); // Setzt voraus, dass dein DTO die Schulden mitliefert
    } catch (error) {
        console.error(error);
        debtsGrid.innerHTML = `<p class="status-text" style="color: #ff4d4d;">Schulden konnten nicht geladen werden.</p>`;
    }
}

// Schulden-Kacheln rendern
function renderDebts(debts) {
    debtsGrid.innerHTML = '';
    
    if (debts.length === 0) {
        debtsGrid.innerHTML = '<p class="status-text">Diese Person hat aktuell keine Schulden! 🎉</p>';
        return;
    }

    debts.forEach(debt => {
        const card = document.createElement('div');
        card.className = 'card';
        // Nutzt Daten aus deinen Debt-Strukturen
        card.innerHTML = `
            <h3>Schuld-Posten</h3>
            <div class="amount">${(debt.amount || 0).toFixed(2)} €</div>
            <div class="details">Eingetragen am: ${debt.createdDate ? new Date(debt.createdDate).toLocaleDateString() : 'Unbekannt'}</div>
        `;
        debtsGrid.appendChild(card);
    });
}

// Platzhalter für deine "Hinzufügen"-Buttons (für spätere Modals/Prompts)
document.getElementById('btn-add-person').addEventListener('click', () => {
    alert('Hier kannst du später ein Modal öffnen, das ein "AddPersonDto" an den POST-Endpunkt schickt!');
});

document.getElementById('btn-add-debt').addEventListener('click', () => {
    alert('Hier kannst du später ein "AddDebtDto" an den DebtController senden!');
});
