function toggleMenu() {
 const menu=document.getElementById('menuMobile'); const open=menu.classList.toggle('open');
 document.getElementById('hamburgerBtn')?.setAttribute('aria-expanded',String(open));
}
function closeMenu() { document.getElementById('menuMobile')?.classList.remove('open'); document.getElementById('hamburgerBtn')?.setAttribute('aria-expanded','false'); }
matchMedia('(min-width: 769px)').addEventListener('change', event => { if(event.matches) closeMenu(); });
document.querySelectorAll('form[data-confirm]').forEach(form=>form.addEventListener('submit',event=>{if(!confirm(form.dataset.confirm))event.preventDefault();}));
document.querySelectorAll('form[data-once]').forEach(form=>form.addEventListener('submit',()=>{form.querySelector('button[type=submit]').disabled=true;}));

document.querySelectorAll('[data-password-toggle]').forEach(button => {
 button.addEventListener('click', () => {
  const input = document.getElementById(button.getAttribute('aria-controls'));
  if (!(input instanceof HTMLInputElement)) return;
  const visible = input.type === 'password';
  input.type = visible ? 'text' : 'password';
  button.textContent = visible ? 'Ẩn' : 'Hiện';
  button.setAttribute('aria-pressed', String(visible));
  const label = input.labels?.[0]?.textContent.trim().toLocaleLowerCase() || 'mật khẩu';
  button.setAttribute('aria-label', `${visible ? 'Ẩn' : 'Hiện'} ${label}`);
 });
});
