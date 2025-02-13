document.addEventListener('DOMContentLoaded', () => {
    const apiBaseUrl = 'http://localhost:5145/api';

    const registerForm = document.getElementById('registerForm');
    const registerMessage = document.getElementById('registerMessage');
    const roleSelect = document.getElementById('role');
    const filmStudioFields = document.getElementById('filmStudioFields');
    const loginForm = document.getElementById('loginForm');
    const loginMessage = document.getElementById('loginMessage');
    const addFilmForm = document.getElementById('addFilmForm');
    const fetchRentedFilmsButton = document.getElementById('fetchRentedFilmsButton');
    const rentedFilmsList = document.getElementById('rentedFilmsList');

    if (roleSelect) {
        roleSelect.addEventListener('change', () => {
            if (roleSelect.value === 'filmstudio') {
                filmStudioFields.style.display = 'block';
            } else {
                filmStudioFields.style.display = 'none';
            }
        });
    }

    if (registerForm) {
        registerForm.addEventListener('submit', async (event) => {
            event.preventDefault();

            const formData = new FormData(registerForm);
            let registerData = {
                username: formData.get('username'),
                password: formData.get('password'),
                role: formData.get('role')
            };

            if (registerData.role === 'filmstudio') {
                registerData = {
                    ...registerData,
                    name: formData.get('name'),
                    city: formData.get('city'),
                    email: formData.get('email')
                };
            }

            try {
                const response = await fetch(`${apiBaseUrl}/user/register`, {
                    method: 'POST',
                    headers: {
                        'Content-Type': 'application/json'
                    },
                    body: JSON.stringify(registerData)
                });

                if (!response.ok) {
                    const errorData = await response.json();
                    throw new Error(errorData.message || 'Registration failed');
                }

                registerMessage.textContent = 'Registration successful!';
                registerForm.reset();
                
                // Redirect to login page after successful registration
                window.location.href = 'login.html';
            } catch (error) {
                console.error('Error:', error);
                registerMessage.textContent = 'Error registering: ' + error.message;
            }
        });
    }

    if (loginForm) {
        loginForm.addEventListener('submit', async (event) => {
            event.preventDefault();

            const loginData = {
                username: loginForm.username.value,
                password: loginForm.password.value
            };

            try {
                const response = await fetch(`${apiBaseUrl}/user/authenticate`, {
                    method: 'POST',
                    headers: {
                        'Content-Type': 'application/json'
                    },
                    body: JSON.stringify(loginData)
                });

                if (!response.ok) {
                    const errorData = await response.json();
                    throw new Error(errorData.message || 'Login failed');
                }

                const data = await response.json();
                // Store JWT token and role in local storage
                localStorage.setItem('jwtToken', data.token);
                localStorage.setItem('role', data.role);

                // Redirect user based on role
                if (data.role === 'admin') {
                    window.location.href = 'admin.html';
                } else if (data.role === 'filmstudio') {
                    window.location.href = 'filmstudio.html';
                }
            } catch (error) {
                console.error('Error:', error);
                loginMessage.textContent = 'Error logging in: ' + error.message;
            }
        });
    }

    if (addFilmForm) {
        addFilmForm.addEventListener('submit', async (event) => {
            event.preventDefault();

            const formData = new FormData(addFilmForm);
            const filmData = {
                title: formData.get('title'),
                description: formData.get('description'),
                availableCopies: parseInt(formData.get('availableCopies')),
                genre: formData.get('genre'),
                director: formData.get('director'),
                year: parseInt(formData.get('year')),
                isAvailable: formData.get('isAvailable') === 'on'
            };

            try {
                const response = await fetch(`${apiBaseUrl}/films`, {
                    method: 'POST',
                    headers: {
                        'Content-Type': 'application/json',
                        'Authorization': `Bearer ${localStorage.getItem('jwtToken')}`
                    },
                    body: JSON.stringify(filmData)
                });

                if (!response.ok) {
                    throw new Error('Failed to add film');
                }

                alert('Film added successfully!');
                addFilmForm.reset();
            } catch (error) {
                console.error('Error:', error);
                alert('Error adding film');
            }
        });
    }

    if (fetchRentedFilmsButton) {
        fetchRentedFilmsButton.addEventListener('click', async () => {
            try {
                const response = await fetch(`${apiBaseUrl}/mystudio/rentals`, {
                    headers: {
                        'Authorization': `Bearer ${localStorage.getItem('jwtToken')}`
                    }
                });

                if (!response.ok) {
                    throw new Error('Failed to fetch rented films');
                }

                const rentedFilms = await response.json();
                rentedFilmsList.innerHTML = '';

                if (rentedFilms.length === 0) {
                    rentedFilmsList.innerHTML = '<p>No rented films found.</p>';
                } else {
                    rentedFilms.forEach(film => {
                        const filmElement = document.createElement('div');
                        filmElement.classList.add('film');
                        filmElement.innerHTML = `
                            <h3>${film.title}</h3>
                            <p>${film.description}</p>
                            <p><strong>Director:</strong> ${film.director}</p>
                            <p><strong>Year:</strong> ${film.year}</p>
                            <p><strong>Genre:</strong> ${film.genre}</p>
                        `;
                        rentedFilmsList.appendChild(filmElement);
                    });
                }
            } catch (error) {
                console.error('Error:', error);
                rentedFilmsList.innerHTML = '<p>Error fetching rented films</p>';
            }
        });
    }
});

function logout() {
    localStorage.removeItem('jwtToken');
    localStorage.removeItem('role');
    window.location.href = 'index.html';
}