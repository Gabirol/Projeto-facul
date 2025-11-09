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
    const chamado = {
        titulo: document.getElementById('titulo').value,
        categoria: document.getElementById('categoria').value,
        descricao: document.getElementById('descricao').value,
        prioridade: document.getElementById('prioridade').value
    };

    // Validação básica
    if (!chamado.titulo || !chamado.categoria || !chamado.descricao) {
        alert('Preencha todos os campos obrigatórios!');
        return;
    }

    // Mostra loading
    const btn = event.target;
    btn.disabled = true;
    btn.textContent = 'Abrindo Chamado...';

    // Envia via AJAX
    fetch('/Chamados/Criar', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
        },
        body: JSON.stringify(chamado)
    })
        .then(response => response.json())
        .then(data => {
            if (data.success) {
                alert('Chamado #' + data.id + ' criado com sucesso!');
                // Limpa o formulário
                document.getElementById('form-chamado').reset();
                // Volta para o dashboard
                showSection('dashboard');
                // Recarrega a lista de chamados se existir
                if (typeof carregarChamados === 'function') {
                    carregarChamados();
                }
            } else {
                alert('Erro: ' + data.message);
            }
        })
        .catch(error => {
            console.error('Erro:', error);
            alert('Erro ao criar chamado');
        })
        .finally(() => {
            // Restaura o botão
            btn.disabled = false;
            btn.textContent = 'Abrir Chamado';
        });
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
