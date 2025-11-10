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

// Funções para mostrar as seções
function showSection(sectionId) {
    // Esconde todas as seções
    document.querySelectorAll('.content-section').forEach(section => {
        section.style.display = 'none';
    });

    // Mostra a seção selecionada
    document.getElementById(sectionId).style.display = 'block';

    // Carrega dados específicos quando a seção é aberta
    if (sectionId === 'ver-chamados') {
        carregarTodosChamados();
    } else if (sectionId === 'meus-chamados') {
        carregarMeusChamados();
    }
}

// Carregar TODOS os chamados
async function carregarTodosChamados() {
    const loading = document.getElementById('loadingTodos');
    const tabela = document.getElementById('tabela-todos-chamados');
    const corpo = document.getElementById('corpo-todos-chamados');
    const semDados = document.getElementById('sem-chamados-todos');

    loading.style.display = 'block';
    tabela.style.display = 'none';
    semDados.style.display = 'none';

    try {
        const response = await fetch('/Chamados/Index');
        if (response.ok) {
            window.location.href = '/Chamados/Index'; // Redireciona para a página dedicada
        } else {
            throw new Error('Erro ao carregar chamados');
        }
    } catch (error) {
        console.error('Erro:', error);
        loading.style.display = 'none';
        semDados.style.display = 'block';
        semDados.innerHTML = '<p>Erro ao carregar chamados.</p>';
    }
}

// Carregar apenas MEUS chamados
async function carregarMeusChamados() {
    const loading = document.getElementById('loadingMeus');
    const tabela = document.getElementById('tabela-meus-chamados');
    const corpo = document.getElementById('corpo-meus-chamados');
    const semDados = document.getElementById('sem-chamados-meus');

    loading.style.display = 'block';
    tabela.style.display = 'none';
    semDados.style.display = 'none';

    try {
        const response = await fetch('/Chamados/MeusChamados');
        if (response.ok) {
            window.location.href = '/Chamados/MeusChamados'; // Redireciona para a página dedicada
        } else {
            throw new Error('Erro ao carregar seus chamados');
        }
    } catch (error) {
        console.error('Erro:', error);
        loading.style.display = 'none';
        semDados.style.display = 'block';
        semDados.innerHTML = '<p>Erro ao carregar seus chamados.</p>';
    }
}

// Funções de ação
function editarChamado(id) {
    if (confirm('Deseja editar este chamado?')) {
        window.location.href = '/Chamados/Editar/' + id;
    }
}

function excluirChamado(id) {
    if (confirm('Tem certeza que deseja excluir este chamado?')) {
        fetch('/Chamados/Excluir/' + id, {
            method: 'DELETE'
        })
            .then(response => response.json())
            .then(data => {
                if (data.success) {
                    alert('Chamado excluído com sucesso!');
                    // Recarrega a listagem atual
                    if (document.getElementById('ver-chamados').style.display !== 'none') {
                        carregarTodosChamados();
                    } else {
                        carregarMeusChamados();
                    }
                } else {
                    alert('Erro: ' + data.message);
                }
            })
            .catch(error => {
                console.error('Erro:', error);
                alert('Erro ao excluir chamado');
            });
    }
}// Funções para mostrar as seções
function showSection(sectionId) {
    // Esconde todas as seções
    document.querySelectorAll('.content-section').forEach(section => {
        section.style.display = 'none';
    });

    // Mostra a seção selecionada
    document.getElementById(sectionId).style.display = 'block';

    // Carrega dados específicos quando a seção é aberta
    if (sectionId === 'ver-chamados') {
        carregarTodosChamados();
    } else if (sectionId === 'meus-chamados') {
        carregarMeusChamados();
    }
}

// Carregar TODOS os chamados
async function carregarTodosChamados() {
    const loading = document.getElementById('loadingTodos');
    const tabela = document.getElementById('tabela-todos-chamados');
    const corpo = document.getElementById('corpo-todos-chamados');
    const semDados = document.getElementById('sem-chamados-todos');

    loading.style.display = 'block';
    tabela.style.display = 'none';
    semDados.style.display = 'none';

    try {
        const response = await fetch('/Chamados/ListarChamados');
        if (response.ok) {
            window.location.href = '/Chamados/ListarChamados';
        } else {
            throw new Error('Erro ao carregar chamados');
        }
    } catch (error) {
        console.error('Erro:', error);
        loading.style.display = 'none';
        semDados.style.display = 'block';
        semDados.innerHTML = '<p>Erro ao carregar chamados.</p>';
    }
}

function editarChamado(id) {
    if (confirm('Deseja editar este chamado?')) {
        window.location.href = '/Chamados/Editar/' + id;
    }
}

function excluirChamado(id) {
    if (confirm('Tem certeza que deseja excluir este chamado?\nEsta ação não pode ser desfeita.')) {
        // Mostrar loading
        const btn = event.target;
        const originalText = btn.innerHTML;
        btn.innerHTML = '🗑️ Excluindo...';
        btn.disabled = true;

        fetch('/Chamados/Excluir/' + id, {
            method: 'DELETE'
        })
            .then(response => response.json())
            .then(data => {
                if (data.success) {
                    alert('Chamado excluído com sucesso!');
                    location.reload(); // Recarrega a página
                } else {
                    alert('Erro: ' + data.message);
                }
            })
            .catch(error => {
                console.error('Erro:', error);
                alert('Erro ao excluir chamado');
            })
            .finally(() => {
                // Restaura o botão
                btn.innerHTML = originalText;
                btn.disabled = false;
            });
    }
}

document.addEventListener('DOMContentLoaded', function () {
    showSection('dashboard');
});
