export function registerFilmStudio(apiBaseUrl) {
    const registerForm = document.getElementById('registerForm');
    const registerMessage = document.getElementById('registerMessage');

    if (registerForm) {
        registerForm.addEventListener('submit', async (event) => {
            event.preventDefault();

            const formData = new FormData(registerForm);
            const registerData = {
                username: formData.get('username'),
                password: formData.get('password'),
                role: formData.get('role'),
                name: formData.get('name'),
                city: formData.get('city'),
                email: formData.get('email')
            };

            // Ensure all required fields for film studio are filled in
            if (registerData.role === 'filmstudio' && (!registerData.name || !registerData.city || !registerData.email)) {
                registerMessage.textContent = 'Please fill in all the required fields for film studio registration.';
                return;
            }

            try {
                const response = await fetch(`${apiBaseUrl}/filmstudio/register`, {
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
                window.location.href = 'login.html';
            } catch (error) {
                console.error('Error:', error);
                registerMessage.textContent = 'Error registering: ' + error.message;
            }
        });
    }

    // Show/hide film studio fields based on role selection
    const roleSelect = document.getElementById('role');
    roleSelect.addEventListener('change', () => {
        const filmStudioFields = document.getElementById('filmStudioFields');
        if (roleSelect.value === 'filmstudio') {
            filmStudioFields.style.display = 'block';
        } else {
            filmStudioFields.style.display = 'none';
        }
    });
}

export function loginFilmStudio(apiBaseUrl) {
    const loginForm = document.getElementById('loginForm');
    const loginMessage = document.getElementById('loginMessage');

    if (loginForm) {
        loginForm.addEventListener('submit', async (event) => {
            event.preventDefault();

            const loginData = {
                username: loginForm.username.value,
                password: loginForm.password.value
            };

            try {
                const response = await fetch(`${apiBaseUrl}/filmstudio/login`, {
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
                localStorage.setItem('jwtToken', data.token);
                localStorage.setItem('role', data.role);
                localStorage.setItem('studioId', data.studioId); // Store the studioId

                if (data.role === 'filmstudio') {
                    window.location.href = 'filmstudio.html';
                }
            } catch (error) {
                console.error('Error:', error);
                loginMessage.textContent = 'Error logging in: ' + error.message;
            }
        });
    }
}

export function fetchRentedFilms(apiBaseUrl) {
    const rentedFilmsList = document.getElementById('rentedFilmsList');
    const studioId = localStorage.getItem('studioId');

    fetch(`${apiBaseUrl}/filmstudio/${studioId}/rented-films`, {
        headers: {
            'Authorization': `Bearer ${localStorage.getItem('jwtToken')}`
        }
    })
    .then(response => {
        if (!response.ok) {
            throw new Error('Failed to fetch rented films');
        }
        return response.json();
    })
    .then(films => {
        rentedFilmsList.innerHTML = '';
        if (films.length === 0) {
            rentedFilmsList.innerHTML = '<p>No rented films available.</p>';
        } else {
            films.forEach(film => {
                const filmElement = document.createElement('div');
                filmElement.classList.add('film');
                filmElement.innerHTML = `
                    <h3>${film.title}</h3>
                    <p>${film.description}</p>
                    <p><strong>Director:</strong> ${film.director}</p>
                    <p><strong>Year:</strong> ${film.year}</p>
                    <p><strong>Genre:</strong> ${film.genre}</p>
                    <p><strong>Available Copies:</strong> ${film.availableCopies}</p>
                `;
                rentedFilmsList.appendChild(filmElement);
            });
        }
    })
    .catch(error => {
        console.error('Error:', error);
        rentedFilmsList.innerHTML = `<p>Error fetching rented films: ${error.message}</p>`;
    });
}

export function fetchAllFilms(apiBaseUrl) {
    const allFilmsList = document.getElementById('allFilmsList');
    const rentFilmMessage = document.createElement('div');
    rentFilmMessage.id = 'rentFilmMessage';
    allFilmsList.parentElement.appendChild(rentFilmMessage);

    fetch(`${apiBaseUrl}/film`, {
        headers: {
            'Authorization': `Bearer ${localStorage.getItem('jwtToken')}`
        }
    })
    .then(response => {
        if (!response.ok) {
            throw new Error('Failed to fetch all films');
        }
        return response.json();
    })
    .then(films => {
        allFilmsList.innerHTML = '';
        if (films.length === 0) {
            allFilmsList.innerHTML = '<p>No films available.</p>';
        } else {
            films.forEach(film => {
                const filmElement = document.createElement('div');
                filmElement.classList.add('film');
                filmElement.innerHTML = `
                    <h3>${film.title}</h3>
                    <p>${film.description}</p>
                    <p><strong>Director:</strong> ${film.director}</p>
                    <p><strong>Year:</strong> ${film.year}</p>
                    <p><strong>Genre:</strong> ${film.genre}</p>
                    <p><strong>Available Copies:</strong> ${film.availableCopies}</p>
                    <button onclick="rentFilm('${film.id}')">Rent</button>
                `;
                allFilmsList.appendChild(filmElement);
            });
        }
    })
    .catch(error => {
        console.error('Error:', error);
        allFilmsList.innerHTML = `<p>Error fetching all films: ${error.message}</p>`;
    });
}

export function rentFilm(filmId) {
    const apiBaseUrl = 'http://localhost:5145/api';
    const studioId = localStorage.getItem('studioId');
    const rentFilmMessage = document.getElementById('rentFilmMessage');

    if (!filmId || !studioId) {
        rentFilmMessage.textContent = 'Invalid film or studio ID.';
        return;
    }

    console.log(`Renting film with filmId: ${filmId} and studioId: ${studioId}`);

    fetch(`${apiBaseUrl}/film/rent?filmId=${filmId}&studioId=${studioId}`, {
        method: 'POST',
        headers: {
            'Authorization': `Bearer ${localStorage.getItem('jwtToken')}`
        }
    })
    .then(response => {
        if (!response.ok) {
            throw new Error(`HTTP error! Status: ${response.status}`);
        }
        return response.json();
    })
    .then(message => {
        rentFilmMessage.textContent = message;
        fetchRentedFilms(apiBaseUrl);
    })
    .catch(error => {
        console.error('Error:', error);
        rentFilmMessage.textContent = `Error renting film: ${error.message}`;
    });
}

export function returnFilm(apiBaseUrl) {
    const returnFilmForm = document.getElementById('returnFilmForm');
    const returnFilmMessage = document.getElementById('returnFilmMessage');

    returnFilmForm.addEventListener('submit', async (event) => {
        event.preventDefault();

        const formData = new FormData(returnFilmForm);
        const returnData = {
            filmCopyId: formData.get('filmCopyId')
        };

        try {
            const response = await fetch(`${apiBaseUrl}/filmstudio/return`, {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                    'Authorization': `Bearer ${localStorage.getItem('jwtToken')}`
                },
                body: JSON.stringify(returnData)
            });

            if (!response.ok) {
                throw new Error('Failed to return film');
            }

            returnFilmMessage.textContent = 'Film returned successfully!';
            fetchRentedFilms(apiBaseUrl);
        } catch (error) {
            console.error('Error:', error);
            returnFilmMessage.textContent = `Error returning film: ${error.message}`;
        }
    });
}