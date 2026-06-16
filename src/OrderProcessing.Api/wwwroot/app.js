const state = { orders: [], selectedId: null, products: [], filter: 'all', search: '' };
const FLOW = ['Pending', 'Confirmed', 'Processing', 'Shipped', 'Delivered'];
const PIPE_ICONS = ['⏳', '✓', '⚙', '🚚', '📦'];
const ACTIONS = {
  Pending: ['pay', 'cancel'],
  Confirmed: ['process', 'cancel'],
  Processing: ['ship', 'cancel'],
  Shipped: ['deliver'],
  Delivered: [],
  Cancelled: []
};
const ACT_META = {
  pay: { icon: '💳', hint: 'Plata' },
  process: { icon: '⚙️', hint: 'Pregatire' },
  ship: { icon: '🚚', hint: 'Expediere' },
  deliver: { icon: '✅', hint: 'Livrare' },
  cancel: { icon: '✕', hint: 'Anulare' }
};

const $ = (sel) => document.querySelector(sel);
const shortId = (id) => '#' + String(id).slice(0, 8);
const fmtTime = (iso) => new Date(iso).toLocaleTimeString('ro-RO', { hour: '2-digit', minute: '2-digit', second: '2-digit' });
const orderId = (o) => (typeof o.id === 'string' ? o.id : o.id.value);
const initials = (name) => name.split(' ').map(w => w[0]).join('').slice(0, 2).toUpperCase();

let toastTimer;
function showToast(msg, ok = true) {
  const t = $('#toast');
  $('#toast-msg').textContent = msg;
  $('#toast-icon').textContent = ok ? '✓' : '✕';
  t.className = `toast ${ok ? 'success' : 'error'}`;
  clearTimeout(toastTimer);
  toastTimer = setTimeout(() => t.classList.add('hidden'), 4500);
}

async function api(path, opts = {}) {
  const res = await fetch(path, { headers: { 'Content-Type': 'application/json' }, ...opts });
  const body = res.status === 204 ? null : await res.json().catch(() => ({}));
  if (!res.ok) {
    const err = new Error(body.message || (body.errors || []).join(' ') || res.statusText);
    err.status = res.status;
    err.body = body;
    throw err;
  }
  return body;
}

function openModal() { $('#modal').classList.remove('hidden'); initForm(); }
function closeModal() { $('#modal').classList.add('hidden'); }

function filteredOrders() {
  return state.orders.filter(o => {
    const id = orderId(o);
    const q = state.search.toLowerCase();
    const matchSearch = !q || id.toLowerCase().includes(q) || o.customer.name.toLowerCase().includes(q);
    const st = (o.status || '').toLowerCase();
    const f = state.filter;
    const matchFilter =
      f === 'all' ||
      (f === 'pending' && st === 'pending') ||
      (f === 'processing' && ['confirmed', 'processing', 'shipped'].includes(st)) ||
      (f === 'delivered' && st === 'delivered') ||
      (f === 'cancelled' && st === 'cancelled');
    return matchSearch && matchFilter;
  });
}

function renderStats() {
  const counts = { total: state.orders.length, pending: 0, active: 0, delivered: 0, cancelled: 0 };
  state.orders.forEach(o => {
    const s = o.status;
    if (s === 'Pending') counts.pending++;
    else if (s === 'Delivered') counts.delivered++;
    else if (s === 'Cancelled') counts.cancelled++;
    else counts.active++;
  });
  $('#stat-total').textContent = counts.total;
  $('#stat-pending').textContent = counts.pending;
  $('#stat-active').textContent = counts.active;
  $('#stat-delivered').textContent = counts.delivered;
  $('#stat-cancelled').textContent = counts.cancelled;
  $('#order-count').textContent = state.orders.length;
}

function renderList() {
  renderStats();
  const list = filteredOrders();
  const empty = $('#list-empty');
  const ul = $('#order-list');
  if (!state.orders.length) {
    ul.innerHTML = '';
    empty.classList.remove('hidden');
    return;
  }
  empty.classList.add('hidden');
  if (!list.length) {
    ul.innerHTML = '<li class="list-empty" style="display:flex"><p>Niciun rezultat pentru filtru.</p></li>';
    return;
  }
  ul.innerHTML = list.map((o, i) => {
    const id = orderId(o);
    const active = id === state.selectedId ? 'active' : '';
    const st = (o.status || '').toLowerCase();
    return `<li class="order-card ${active}" data-id="${id}" style="animation-delay:${i * 40}ms">
      <div class="order-card-top">
        <span class="order-card-id">${shortId(id)}</span>
        <span class="badge ${st}">${o.status}</span>
      </div>
      <div class="order-card-meta">
        <span>${o.customer.name}</span>
        <span class="order-card-total">${o.total.amount} RON</span>
      </div>
    </li>`;
  }).join('');
}

function renderPipeline(status) {
  if (status === 'Cancelled') {
    return `<div class="pipeline"><div class="pipe-step cancelled current"><div class="pipe-dot">✕</div><span class="pipe-label">Cancelled</span></div></div>
      <div class="cancel-branch">Comanda <strong>anulata</strong> - flux oprit.</div>`;
  }
  const idx = FLOW.indexOf(status);
  return `<div class="pipeline">${FLOW.map((s, i) => {
    let cls = 'pipe-step';
    if (i < idx) cls += ' done';
    if (s === status) cls += ' current';
    return `<div class="${cls}"><div class="pipe-dot">${PIPE_ICONS[i]}</div><span class="pipe-label">${s}</span></div>`;
  }).join('')}</div>`;
}

function renderActions(status) {
  const allowed = ACTIONS[status] || [];
  return ['pay', 'process', 'ship', 'deliver', 'cancel'].map(a => {
    const on = allowed.includes(a);
    const m = ACT_META[a];
    const cls = `action-card${on ? ' enabled' : ''}${a === 'cancel' ? ' cancel-type' : ''}`;
    return `<button type="button" class="${cls}" data-action="${a}" ${on ? '' : 'disabled'}>
      <span class="act-icon">${m.icon}</span>
      <span class="act-name">${a[0].toUpperCase() + a.slice(1)}</span>
      <span class="act-hint">${m.hint}</span>
    </button>`;
  }).join('');
}

function renderDetails(order) {
  const el = $('#details');
  if (!order) {
    el.innerHTML = `<div class="empty-hero"><div class="empty-ring">◎</div>
      <h2>Selecteaza o comanda</h2>
      <p>Alege din panoul din stanga sau creeaza o comanda noua pentru a vedea pipeline-ul de stari si actiunile disponibile.</p></div>`;
    return;
  }
  const id = orderId(order);
  const st = (order.status || '').toLowerCase();
  const itemsRows = order.items.map(i =>
    `<tr><td>${i.productName}</td><td>${i.quantity}</td><td class="price">${i.unitPrice.amount} RON</td></tr>`
  ).join('');
  const hist = (order.history || []).filter(h => h.fromState !== '-').map((h, i) =>
    `<div class="tl-item" style="animation-delay:${i * 60}ms"><span class="trn">${h.fromState} -> ${h.toState}</span><span class="ts">${fmtTime(h.at)}</span></div>`
  ).join('') || '<p style="color:var(--text-dim);font-size:0.82rem">Nicio tranzitie inca.</p>';

  el.innerHTML = `<div class="detail-layout">
    <header class="detail-hero">
      <div class="avatar">${initials(order.customer.name)}</div>
      <div class="hero-main">
        <h1>${id}</h1>
        <h2>${order.customer.name}</h2>
        <p>${order.customer.email} - ${order.customer.age} ani${order.customer.isTrusted ? ' - trusted' : ''}</p>
      </div>
      <div class="status-hero">
        <span class="badge ${st}">${order.status}</span>
        <span class="total-big">${order.total.amount} ${order.total.currency}</span>
      </div>
    </header>
    <div class="card"><div class="card-title">Pipeline stari</div>${renderPipeline(order.status)}</div>
    <div class="detail-grid-2">
      <div class="card"><div class="card-title">Produse</div>
        <table class="items-table"><thead><tr><th>Produs</th><th>Qty</th><th>Pret</th></tr></thead><tbody>${itemsRows}</tbody></table>
      </div>
      <div class="card"><div class="card-title">Livrare</div>
        <dl class="info-rows">
          <div class="info-row"><dt>Strada</dt><dd>${order.shippingAddress.street}</dd></div>
          <div class="info-row"><dt>Oras</dt><dd>${order.shippingAddress.city}</dd></div>
          <div class="info-row"><dt>Cod postal</dt><dd>${order.shippingAddress.postalCode}</dd></div>
          <div class="info-row"><dt>Tara</dt><dd>${order.shippingAddress.country}</dd></div>
        </dl>
      </div>
    </div>
    <div class="card"><div class="card-title">Actiuni disponibile</div>
      <div class="action-deck">${renderActions(order.status)}</div>
    </div>
    <div class="card"><div class="card-title">Istoric tranzitii</div><div class="timeline">${hist}</div></div>
  </div>`;

  el.querySelectorAll('[data-action]').forEach(btn => {
    if (!btn.disabled) btn.onclick = async () => {
      try {
        const updated = await triggerAction(id, btn.dataset.action);
        showToast(`Tranzitie reusita -> ${updated.status}`);
        await fetchOrders();
      } catch (e) { showToast(e.message || 'Eroare la tranzitie', false); }
    };
  });
}

async function fetchOrders() {
  state.orders = await api('/orders');
  renderList();
  if (state.selectedId && !state.orders.find(o => orderId(o) === state.selectedId))
    state.selectedId = state.orders[0] ? orderId(state.orders[0]) : null;
  if (state.selectedId) await fetchOrder(state.selectedId);
  else renderDetails(null);
}

async function fetchOrder(id) {
  state.selectedId = id;
  const order = await api(`/orders/${id}`);
  renderList();
  renderDetails(order);
  return order;
}

async function createOrder(data) {
  return api('/orders', { method: 'POST', body: JSON.stringify(data) });
}

async function triggerAction(id, action) {
  return api(`/orders/${id}/${action}`, { method: 'POST' });
}

function calcTotal() {
  let sum = 0;
  document.querySelectorAll('.item-row').forEach(row => {
    sum += Number(row.querySelector('[name=qty]').value) * Number(row.querySelector('[name=price]').value);
  });
  $('#form-total').textContent = sum.toFixed(2);
  return sum;
}

function addItemRow(product) {
  const div = document.createElement('div');
  div.className = 'item-row';
  const opts = state.products.map(p =>
    `<option value="${p.id}" data-price="${p.defaultPrice}" data-age="${p.hasAgeRestriction}" data-name="${p.name}" ${product?.id === p.id ? 'selected' : ''}>${p.name}</option>`
  ).join('');
  div.innerHTML = `
    <select name="product">${opts}</select>
    <input name="qty" type="number" min="1" value="1" required title="Cantitate">
    <input name="price" type="number" min="0.01" step="0.01" value="${product?.defaultPrice ?? 100}" required title="Pret">
    <label class="age-lbl"><input name="age" type="checkbox" ${product?.hasAgeRestriction ? 'checked' : ''}>18+</label>
    <button type="button" class="rm" title="Sterge">✕</button>`;
  div.querySelector('select').onchange = (e) => {
    const o = e.target.selectedOptions[0];
    div.querySelector('[name=price]').value = o.dataset.price;
    div.querySelector('[name=age]').checked = o.dataset.age === 'true';
    calcTotal();
  };
  div.querySelectorAll('input').forEach(i => i.oninput = calcTotal);
  div.querySelector('.rm').onclick = () => { div.remove(); calcTotal(); };
  $('#items-container').appendChild(div);
  calcTotal();
}

async function initForm() {
  state.products = await api('/products');
  $('#items-container').innerHTML = '';
  addItemRow(state.products[0]);
}

$('#order-list').onclick = (e) => {
  const row = e.target.closest('[data-id]');
  if (row) fetchOrder(row.dataset.id).catch(err => showToast(err.message, false));
};

$('#search').oninput = (e) => { state.search = e.target.value; renderList(); };

$('#filter-chips').onclick = (e) => {
  const chip = e.target.closest('.chip');
  if (!chip) return;
  $('#filter-chips .chip').forEach(c => c.classList.remove('active'));
  chip.classList.add('active');
  state.filter = chip.dataset.filter;
  renderList();
};

$('#btn-new').onclick = openModal;
$('#btn-empty-new').onclick = openModal;
$('#btn-cancel-modal').onclick = closeModal;
$('#btn-modal-cancel').onclick = closeModal;
$('#modal-backdrop').onclick = closeModal;
$('#btn-add-item').onclick = () => addItemRow();
$('#toast-close').onclick = () => $('#toast').classList.add('hidden');

$('#create-form').onsubmit = async (e) => {
  e.preventDefault();
  const f = e.target;
  const items = [...document.querySelectorAll('.item-row')].map(row => {
    const sel = row.querySelector('select').selectedOptions[0];
    return {
      productId: sel.value,
      productName: sel.dataset.name,
      quantity: Number(row.querySelector('[name=qty]').value),
      unitPrice: Number(row.querySelector('[name=price]').value),
      hasAgeRestriction: row.querySelector('[name=age]').checked
    };
  });
  const payload = {
    customer: { name: f.name.value, email: f.email.value, age: Number(f.age.value), isTrusted: f.trusted.checked },
    address: { street: f.street.value, city: f.city.value, postalCode: f.postal.value, country: f.country.value },
    items, total: calcTotal()
  };
  const errBox = $('#form-errors');
  errBox.classList.add('hidden');
  try {
    const order = await createOrder(payload);
    closeModal();
    showToast(`Comanda creata ${shortId(orderId(order))}`);
    state.selectedId = orderId(order);
    await fetchOrders();
  } catch (err) {
    const msgs = err.body?.errors || [err.message];
    errBox.innerHTML = msgs.map(m => `<div>${m}</div>`).join('');
    errBox.classList.remove('hidden');
    showToast(msgs[0], false);
  }
};

fetchOrders().catch(err => showToast(err.message, false));
