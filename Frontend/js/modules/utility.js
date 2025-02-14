export function logout() {
    localStorage.removeItem('jwtToken');
    localStorage.removeItem('role');
    window.location.href = 'index.html';
}