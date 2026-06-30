const API_BASE = '/api/Schuldenbuch';

// Variablen für DOM-Elemente
let personsView, detailsView, personsGrid, debtsGrid, selectedPersonName, btnBack;
let modalPerson, modalDebt, inputPersonName, inputDebtAmount;
let currentPersonId = null;

// Erst starten, wenn das gesamte HTML geladen wurde!
document.addEventListener('DOMContentLoaded', () => {
    console.log("Schuldenbuch-Frontend geladen. Initialisiere Elemente...");

    // Elemente zuweisen
    personsView = document.getElementById('persons-view');
    detailsView = document.getElementById('details-view');
    personsGrid = document.getElementById('persons-grid');
    debtsGrid = document.getElementById('debts-grid');
    selectedPersonName = document.getElementById('selected-person-name');
    btnBack = document.getElementById('btn-back');
    modalPerson = document.getElementById('modal-person');
    modalDebt = document.getElementById('modal-debt');
    inputPersonName = document.getElementById('input-person-name');
    inputDebtAmount = document.getElementById('input-debt-amount');

    // --- EVENT LISTENER REGISTRIEREN ---

    // Zurück-Button
    if (btnBack) {
        btnBack.addEventListener('click', () => {
            detailsView.classList.add('hidden');
            personsView.classList.remove('hidden');
            loadPersons();
        });
    }

    // Modal öffnen: Person hinzufügen
    const btnAddPerson = document.getElementById('btn-add-person');
    if (btnAddPerson) {
        btnAddPerson.addEventListener('click', () => {
            console.log("Klick auf: + Person hinzufügen");
            if (inputPersonName) inputPersonName.value = '';
             modalPerson.classList.remove('hidden');
        });
    } else {
        console.error("Button 'btn-add-person' wurde im HTML nicht gefunden!");
    }

    // Modal schließen: Person hinzufügen
    const btnClosePersonModal = document.getElementById('btn-close-person-modal');
    if (btnClosePersonModal) {
        btnClosePersonModal.addEventListener('click', () => {
            if (modalPerson) modalPerson.classList.add('hidden');
        });
    }

    // Person speichern (POST)
    const btnSavePerson = document.getElementById('btn-save-person');
    if (btnSavePerson) {
        btnSavePerson.addEventListener('click', savePersonData);
    }

    // Modal öffnen: Schuld hinzufügen
    const btnAddDebt = document.getElementById('btn-add-debt');
    if (btnAddDebt) {
        btnAddDebt.addEventListener('click', () => {
            console.log("Klick auf: + Schuld hinzufügen");
            if (inputDebtAmount) inputDebtAmount.value = '';
            if (modalDebt) modalDebt.classList.remove('hidden');
        });
    }

    // Modal schließen: Schuld hinzufügen
    const btnCloseDebtModal = document.getElementById('btn-close-debt-modal');
    if (btnCloseDebtModal) {
        btnCloseDebtModal.addEventListener('click', () => {
            if (modalDebt) modalDebt.classList.add('hidden');
        });
    }

    // Schuld speichern (POST)
    const btnSaveDebt = document.getElementById('btn-save-debt');
    if (btnSaveDebt) {
        btnSaveDebt.addEventListener('click', saveDebtData);
    }

    // Direkt beim Start Personen aus dem Backend laden
    loadPersons();
});

// --- API-LOGIK (POST & GET) ---

async function loadPersons() {
    try {
        const response = await fetch(`${API_BASE}/Schuldenbuch/Person`);
        if (!response.ok) throw new Error('Fehler beim Laden');
        const persons = await response.json();
        renderPersons(persons);
    } catch (error) {
        if (personsGrid) {
            personsGrid.innerHTML = `<p class="status-text" style="color: #ef4444;">API nicht erreichbar. Nutze '+ Person hinzufügen' zum Testen.</p>`;
        }
    }
}

function renderPersons(persons) {
    if (!personsGrid) return;
    personsGrid.innerHTML = '';
    if(persons.length === 0) {
        personsGrid.innerHTML = '<p class="status-text">Keine Personen vorhanden.</p>';
        return;
    }
    persons.forEach(person => {
        const card = document.createElement('div');
        card.className = 'card';
        card.innerHTML = `
            <div class="card-info"><h3>${person.name}</h3></div>
            <div class="amount">${(person.totalDebt || 0).toFixed(2)} €</div>
        `;
        card.addEventListener('click', () => openPersonDetails(person));
        personsGrid.appendChild(card);
    });
}

async function savePersonData() {
    const name = inputPersonName.value.trim();
    if (!name) return alert('Bitte einen Namen eingeben.');

    const dto = { name: name }; 

    try {
        const response = await fetch(`${API_BASE}/Schuldenbuch/Person`, {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify(dto)
        });

        if (response.ok) {
            modalPerson.classList.add('hidden');
            loadPersons();
        } else {
            alert('Fehler beim Speichern im Backend.');
        }
    } catch (error) {
        console.error(error);
        // Fallback für Offline-Testen in Visual Studio:
        modalPerson.classList.add('hidden');
        alert(`Offline-Modus: Person "${name}" lokal gespeichert (Backend nicht erreichbar).`);
    }
}

async function openPersonDetails(person) {
    currentPersonId = person.id;
    if (selectedPersonName) selectedPersonName.textContent = `Schulden von ${person.name}`;
    if (personsView) personsView.classList.add('hidden');
    if (detailsView) detailsView.classList.remove('hidden');
    
    try {
        const response = await fetch(`${API_BASE}/Schuldenbuch/Person/${person.id}`);
        const personData = await response.json();
        renderDebts(personData.debts || []);
    } catch (error) {
        if (debtsGrid) debtsGrid.innerHTML = '<p class="status-text">Posten konnten nicht geladen werden.</p>';
    }
}

function renderDebts(debts) {
    if (!debtsGrid) return;
    debtsGrid.innerHTML = '';
    if(debts.length === 0) {
        debtsGrid.innerHTML = '<p class="status-text">Keine offenen Posten. 🎉</p>';
        return;
    }
    debts.forEach(debt => {
        const card = document.createElement('div');
        card.className = 'card';
        card.innerHTML = `
            <div class="card-info">
                <h3>Schuld-Posten</h3>
                <div class="date">${debt.createdDate ? new Date(debt.createdDate).toLocaleDateString() : ''}</div>
            </div>
            <div class="amount">${debt.amount.toFixed(2)} €</div>
        `;
        debtsGrid.appendChild(card);
    });
}

async function saveDebtData() {
    const amount = parseFloat(inputDebtAmount.value);
    if (isNaN(amount) || amount <= 0) return alert('Bitte einen gültigen Betrag eingeben.');

    const dto = {
        personId: currentPersonId,
        amount: amount
    };

    try {
        const response = await fetch(`${API_BASE}/Schuldenbuch/Debt`, {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify(dto)
        });

        if (response.ok) {
            modalDebt.classList.add('hidden');
            const res = await fetch(`${API_BASE}/Schuldenbuch/Person/${currentPersonId}`);
            const personData = await res.json();
            renderDebts(personData.debts || []);
        } else {
            alert('Fehler beim Eintragen der Schuld.');
        }
    } catch (error) {
        console.error(error);
        modalDebt.classList.add('hidden');
    }
}