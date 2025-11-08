function showSection(sectionId) {
    document.querySelectorAll('.content-section').forEach(section => {
        section.style.display = 'none';
    });
    document.getElementById(sectionId).style.display = 'block';

    document.querySelectorAll('.nav-item').forEach(item => {
        item.classList.remove('active');
    });
    event.currentTarget.classList.add('active');
}

function criarChamado() {
    const titulo = document.getElementById('titulo').value;
    const descricao = document.getElementById('descricao').value;

    if (titulo && descricao) {
        alert('Chamado criado com sucesso!');
        showSection('meus-chamados');
    } else {
        alert('Preencha todos os campos obrigatórios!');
    }
}

function editarChamado(id) {
    alert(`Editando chamado #${id}`);
}

function excluirChamado(id) {
    if (confirm('Tem certeza que deseja excluir este chamado?')) {
        alert(`Chamado #${id} excluído!`);
    }
}

document.addEventListener('DOMContentLoaded', function () {
    showSection('dashboard');
});
