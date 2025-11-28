const modal = document.getElementById('modal-foto');
const botaoAbrir = document.getElementById('acao-foto');
const botaoFechar = document.getElementById('fecharModalFoto');

botaoAbrir.addEventListener('click', () => modal.showModal());
botaoFechar.addEventListener('click', () => modal.close());

document.getElementById('upload-foto').addEventListener('change', e => {
    const arquivo = e.target.files[0];
    if (arquivo) {
        document.getElementById('preview-foto').src = URL.createObjectURL(arquivo);
    }
});
