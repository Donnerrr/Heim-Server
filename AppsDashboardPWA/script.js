const API_BASE = 'http://localhost:5000/api/Schuldenbuch';

let selectedPersonId = null;

function showTab(tab) {
    document.querySelectorAll('.tab-content').forEach(el => el.style.display = 'none');
    document.getElementById(`tab-content-${tab}`).style.display = 'block';
    
    document.querySelectorAll('nav button').forEach((btn, i) => {
        btn.classList.toggle('active', i === tab);
    });
    
    if (tab === 1) loadPersons();
}

function showToast(message, isError = false) {
    const toast = document.getElementById('toast');
    toast.textContent = message;
    toast.style.background = isError ? '#e11d48' : '#00b4d8';
    toast.style.color = isError ? 'white' : '#0a2540';
    toast.style.display = 'block';
    setTimeout(() => toast.style.display = 'none', 3000);
}

// ==================== PERSONEN ====================
async function loadPersons() {
    try {
        const res = await fetch(`${API_BASE}/Person`);
        const persons = res.ok ? await res.json() : [];
        
        const grid = document.getElementById('persons-grid');
        grid.innerHTML = persons.map(p => `
            <div class="person-card" onclick="showPersonDetail(${p.id})">
                <h3>${p.firstName} ${p.lastName}</h3>
                <p><strong>Email:</strong> ${p.email || '-'}</p>
                <p><strong>Telefon:</strong> ${p.phone || '-'}</p>
                <button onclick="event.stopImmediatePropagation(); deletePerson(${p.id});" 
                        class="btn btn-danger" style="margin-top: 1rem; width: 100%;">
                    Löschen
                </button>
            </div>
        `).join('');
        
        document.getElementById('total-persons').textContent = persons.length;
    } catch (e) {
        showToast('Fehler beim Laden der Personen', true);
    }
}

async function showPersonDetail(id) {
    selectedPersonId = id;
    try {
        const res = await fetch(`${API_BASE}/Person/${id}`);
        if (!res.ok) throw new Error();
        const person = await res.json();

        document.getElementById('detail-person-name').textContent = 
            `${person.firstName} ${person.lastName}`;

        const debts = person.debts || person.schulden || []; // je nach Backend-Feldname

        const grid = document.getElementById('debts-grid');
        grid.innerHTML = debts.length ? debts.map(d => `
            <div class="debt-card">
                <h4>${parseFloat(d.amount).toFixed(2)} €</h4>
                <p><strong>Beschreibung:</strong> ${d.description || '-'}</p>
                <p><strong>Datum:</strong> ${d.date ? new Date(d.date).toLocaleDateString('de-DE') : '-'}</p>
                <div class="card-actions">
                    <button onclick="updateDebt(${d.id}, 10, true)" class="btn">+10 €</button>
                    <button onclick="updateDebt(${d.id}, 10, false)" class="btn btn-danger">-10 €</button>
                    <button onclick="deleteDebt(${d.id})" class="btn btn-danger">Löschen</button>
                </div>
            </div>
        `).join('') : '<p>Keine Schulden vorhanden.</p>';

        document.getElementById('person-detail-modal').style.display = 'flex';
    } catch (e) {
        showToast('Fehler beim Laden der Details', true);
    }
}

// ==================== CRUD ====================
async function addPerson() { /* ... wie vorher */ }
async function deletePerson(id) { /* ... wie vorher */ }
async function addDebt() { /* ... wie vorher, mit selectedPersonId */ }
async function deleteDebt(id) { /* ... */ }
async function updateDebt(id, amount, isAddition) { /* ... */ }

function showAddPersonModal() {
    document.getElementById('person-modal').style.display = 'flex';
}

function showAddDebtModal() {
    document.getElementById('debt-modal').style.display = 'flex';
}

function closeModals() {
    document.querySelectorAll('.modal').forEach(m => m.style.display = 'none');
}

// Init
window.onload = () => {
    showTab(0);
    setTimeout(loadPersons, 500);
};