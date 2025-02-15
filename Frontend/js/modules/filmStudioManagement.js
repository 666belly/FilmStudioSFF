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
                localStorage.setItem('filmStudioId', data.filmStudioId); 
                console.log('Logged in with filmStudioId:', data.filmStudioId); 

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


export function fetchRentedFilms(apiBaseUrl, filmStudioId) {
    const rentedFilmsList = document.getElementById('rentedFilmsList');

    if (!filmStudioId) {
        console.error('No filmStudioId provided');
        rentedFilmsList.innerHTML = '<p>Error: No filmStudioId provided.</p>';
        return;
    }

    fetch(`${apiBaseUrl}/filmstudio/${filmStudioId}/rented-films`, {
        headers: {
            'Authorization': `Bearer ${localStorage.getItem('jwtToken')}`
        }
    })
    .then(response => {
        if (!response.ok) {
            return response.text().then(text => { throw new Error(text || response.statusText); });
        }
        return response.json();
    })
    .then(rentedFilms => {
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
    })
    .catch(error => {
        console.error('Error:', error);
        rentedFilmsList.innerHTML = `<p>Error fetching rented films: ${error.message}</p>`;
    });
}

export function rentFilmToStudio(apiBaseUrl, filmStudioId, filmId) {
    const rentFilmMessage = document.getElementById('rentFilmMessage');

    if (!filmStudioId || !filmId) {
        console.error('Missing filmStudioId or filmId');
        rentFilmMessage.textContent = 'Error: Missing filmStudioId or filmId';
        return;
    }

    console.log(`Renting film with filmId: ${filmId} to studio with filmStudioId: ${filmStudioId}`);

    fetch(`${apiBaseUrl}/film/rent?filmId=${filmId}&studioId=${filmStudioId}`, {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
            'Authorization': `Bearer ${localStorage.getItem('jwtToken')}`
        }
    })
    .then(response => {
        if (!response.ok) {
            return response.text().then(text => { throw new Error(text || response.statusText); });
        }
        return response.text().then(text => text ? JSON.parse(text) : {});
    })
    .then(data => {
        rentFilmMessage.textContent = 'Film rented successfully!';
        fetchRentedFilms(apiBaseUrl, filmStudioId); 
    })
    .catch(error => {
        console.error('Error:', error);
        rentFilmMessage.textContent = `Error renting film: ${error.message}`;
    });
}
// export function fetchAllFilms(apiBaseUrl) {
//     const filmList = document.getElementById('filmList');
//     const headers = {};
//     const filmStudioId = localStorage.getItem('filmStudioId'); 
//     const token = localStorage.getItem('jwtToken');
//     if (token) {
//         headers['Authorization'] = `Bearer ${token}`;
//     }

//     fetch(`${apiBaseUrl}/film`, { headers })
//     .then(response => {
//         if (!response.ok) {
//             throw new Error('Failed to fetch films');
//         }
//         return response.json();
//     })
//     .then(films => {
//         const filmsArray = films.$values || films;
//         if (!Array.isArray(filmsArray)) {
//             throw new Error('Films response is not an array');
//         }

//         filmList.innerHTML = '';
//         if (filmsArray.length === 0) {
//             filmList.innerHTML = '<p>No films available.</p>';
//         } else {
//             filmsArray.forEach(film => {
//                 const filmElement = document.createElement('div');
//                 filmElement.classList.add('film');
//                 filmElement.innerHTML = `
//                     <h3>${film.title}</h3>
//                     <p>${film.description}</p>
//                     <p><strong>Director:</strong> ${film.director}</p>
//                     <p><strong>Year:</strong> ${film.year}</p>
//                     <p><strong>Genre:</strong> ${film.genre}</p>
//                     <p><strong>Available Copies:</strong> ${film.availableCopies}</p>
//                 `;
//                 filmList.appendChild(filmElement);
//             });
//         }
//     })
//     .catch(error => {
//         console.error('Error:', error);
//         filmList.innerHTML = `<p>Error fetching films: ${error.message}</p>`;
//     });
// }