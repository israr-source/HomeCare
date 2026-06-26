document.querySelectorAll('[data-password-toggle]').forEach(function (button) {
    button.addEventListener('click', function () {
        var field = button.closest('.password-field');
        if (!field) {
            return;
        }

        var input = field.querySelector('input');
        if (!input) {
            return;
        }

        var isPassword = input.type === 'password';
        input.type = isPassword ? 'text' : 'password';
        button.textContent = isPassword ? 'Hide' : 'Show';
        button.setAttribute('aria-label', isPassword ? 'Hide password' : 'Show password');
    });
});
