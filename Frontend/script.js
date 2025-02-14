// document.addEventListener('DOMContentLoaded', () => {
//     const apiBaseUrl = 'http://localhost:5145/api';
//     const isAdminPage = window.location.pathname.includes('admin.html');

//     const registerForm = document.getElementById('registerForm');
//     const registerMessage = document.getElementById('registerMessage');
//     const roleSelect = document.getElementById('role');
//     const filmStudioFields = document.getElementById('filmStudioFields');
//     const loginForm = document.getElementById('loginForm');
//     const loginMessage = document.getElementById('loginMessage');
//     const addFilmForm = document.getElementById('addFilmForm');
//     const fetchRentedFilmsButton = document.getElementById('fetchRentedFilmsButton');
//     const rentedFilmsList = document.getElementById('rentedFilmsList');
//     const getAllUsersButton = document.getElementById('getAllUsersButton');
//     const getUserButton = document.getElementById('getUserButton');
//     const userIdInput = document.getElementById('userIdInput');
//     const userList = document.getElementById('userList');
//     const userDetails = document.getElementById('userDetails');
//     const getAllFilmStudiosButton = document.getElementById('getAllFilmStudiosButton');
//     const getFilmStudioButton = document.getElementById('getFilmStudioButton');
//     const filmStudioIdInput = document.getElementById('filmStudioIdInput');
//     const filmStudioList = document.getElementById('filmStudioList');
//     const filmStudioDetails = document.getElementById('filmStudioDetails');
//     const toggleAddFilmFormButton = document.getElementById('toggleAddFilmForm');
//     const filmList = document.getElementById('filmList');

//     if (toggleAddFilmFormButton) {
//         toggleAddFilmFormButton.addEventListener('click', () => {
//             if (addFilmForm.style.display === 'none') {
//                 addFilmForm.style.display = 'block';
//             } else {
//                 addFilmForm.style.display = 'none';
//             }
//         });
//     }

//     if (roleSelect) {
//         roleSelect.addEventListener('change', () => {
//             if (roleSelect.value === 'filmstudio') {
//                 filmStudioFields.style.display = 'block';
//             } else {
//                 filmStudioFields.style.display = 'none';
//             }
//         });
//     }

//     if (registerForm) {
//         registerForm.addEventListener('submit', async (event) => {
//             event.preventDefault();

//             const formData = new FormData(registerForm);
//             let registerData = {
//                 username: formData.get('username'),
//                 password: formData.get('password'),
//                 role: formData.get('role')
//             };

//             if (registerData.role === 'filmstudio') {
//                 registerData = {
//                     ...registerData,
//                     name: formData.get('name'),
//                     city: formData.get('city'),
//                     email: formData.get('email')
//                 };
//             }

//             try {
//                 const response = await fetch(`${apiBaseUrl}/user/register`, {
//                     method: 'POST',
//                     headers: {
//                         'Content-Type': 'application/json'
//                     },
//                     body: JSON.stringify(registerData)
//                 });

//                 if (!response.ok) {
//                     const errorData = await response.json();
//                     throw new Error(errorData.message || 'Registration failed');
//                 }

//                 registerMessage.textContent = 'Registration successful!';
//                 registerForm.reset();
                
//                 // Redirect to login page after successful registration
//                 window.location.href = 'login.html';
//             } catch (error) {
//                 console.error('Error:', error);
//                 registerMessage.textContent = 'Error registering: ' + error.message;
//             }
//         });
//     }

//     if (loginForm) {
//         loginForm.addEventListener('submit', async (event) => {
//             event.preventDefault();

//             const loginData = {
//                 username: loginForm.username.value,
//                 password: loginForm.password.value
//             };

//             try {
//                 const response = await fetch(`${apiBaseUrl}/user/authenticate`, {
//                     method: 'POST',
//                     headers: {
//                         'Content-Type': 'application/json'
//                     },
//                     body: JSON.stringify(loginData)
//                 });

//                 if (!response.ok) {
//                     const errorData = await response.json();
//                     throw new Error(errorData.message || 'Login failed');
//                 }

//                 const data = await response.json();
//                 // Store JWT token and role in local storage
//                 localStorage.setItem('jwtToken', data.token);
//                 localStorage.setItem('role', data.role);

//                 // Redirect user based on role
//                 if (data.role === 'admin') {
//                     window.location.href = 'admin.html';
//                 } else if (data.role === 'filmstudio') {
//                     window.location.href = 'filmstudio.html';
//                 }
//             } catch (error) {
//                 console.error('Error:', error);
//                 loginMessage.textContent = 'Error logging in: ' + error.message;
//             }
//         });
//     }

//     if (addFilmForm) {
//         addFilmForm.addEventListener('submit', async (event) => {
//             event.preventDefault();

//             const formData = new FormData(addFilmForm);
//             const filmData = {
//                 title: formData.get('title'),
//                 description: formData.get('description'),
//                 availableCopies: parseInt(formData.get('availableCopies')),
//                 genre: formData.get('genre'),
//                 director: formData.get('director'),
//                 year: parseInt(formData.get('year')),
//                 isAvailable: formData.get('isAvailable') === 'on'
//             };

//             try {
//                 const response = await fetch(`${apiBaseUrl}/film`, {
//                     method: 'POST',
//                     headers: {
//                         'Content-Type': 'application/json',
//                         'Authorization': `Bearer ${localStorage.getItem('jwtToken')}`
//                     },
//                     body: JSON.stringify(filmData)
//                 });

//                 if (!response.ok) {
//                     if (response.status === 403) {
//                         throw new Error('User does not have permission to add films. Please ensure you are logged in as an Admin.');
//                     }
//                     const errorData = await response.json();
//                     console.error('Error adding film:', errorData);
//                     throw new Error('Failed to add film');
//                 }

//                 alert('Film added successfully!');
//                 addFilmForm.reset();
//                 // refresh film list
//                 fetchAllFilms();
//             } catch (error) {
//                 console.error('Error:', error);
//                 alert(`Error adding film: ${error.message}`);
//             }
//         });
//     }

//     if (fetchRentedFilmsButton) {
//         fetchRentedFilmsButton.addEventListener('click', async () => {
//             try {
//                 const response = await fetch(`${apiBaseUrl}/mystudio/rentals`, {
//                     headers: {
//                         'Authorization': `Bearer ${localStorage.getItem('jwtToken')}`
//                     }
//                 });

//                 if (!response.ok) {
//                     throw new Error('Failed to fetch rented films');
//                 }

//                 const rentedFilms = await response.json();
//                 rentedFilmsList.innerHTML = '';

//                 if (rentedFilms.length === 0) {
//                     rentedFilmsList.innerHTML = '<p>No rented films found.</p>';
//                 } else {
//                     rentedFilms.forEach(film => {
//                         const filmElement = document.createElement('div');
//                         filmElement.classList.add('film');
//                         filmElement.innerHTML = `
//                             <h3>${film.title}</h3>
//                             <p>${film.description}</p>
//                             <p><strong>Director:</strong> ${film.director}</p>
//                             <p><strong>Year:</strong> ${film.year}</p>
//                             <p><strong>Genre:</strong> ${film.genre}</p>
//                         `;
//                         rentedFilmsList.appendChild(filmElement);
//                     });
//                 }
//             } catch (error) {
//                 console.error('Error:', error);
//                 rentedFilmsList.innerHTML = '<p>Error fetching rented films</p>';
//             }
//         });
//     }

//     if (getAllUsersButton) {
//         getAllUsersButton.addEventListener('click', async () => {
//             try {
//                 const response = await fetch(`${apiBaseUrl}/user/all`, {
//                     headers: {
//                         'Authorization': `Bearer ${localStorage.getItem('jwtToken')}`
//                     }
//                 });

//                 if (!response.ok) {
//                     throw new Error('Failed to fetch users');
//                 }

//                 const users = await response.json();
//                 const usersArray = users.$values || users;
//                 if (!Array.isArray(usersArray)) {
//                     throw new Error('Users response is not an array');
//                 }

//                 userList.innerHTML = '';

//                 if (usersArray.length === 0) {
//                     userList.innerHTML = '<p>No users found.</p>';
//                 } else {
//                     usersArray.forEach(user => {
//                         const userElement = document.createElement('div');
//                         userElement.classList.add('user');
//                         userElement.innerHTML = `
//                             <p><strong>Username:</strong> ${user.username}</p>
//                             <p><strong>Role:</strong> ${user.role}</p>
//                         `;
//                         userList.appendChild(userElement);
//                     });
//                 }
//             } catch (error) {
//                 console.error('Error:', error);
//                 userList.innerHTML = '<p>Error fetching users</p>';
//             }
//         });
//     }

//     if (getUserButton) {
//         getUserButton.addEventListener('click', async () => {
//             const userId = userIdInput.value;
//             if (!userId) {
//                 alert('Please enter a user ID');
//                 return;
//             }

//             try {
//                 const response = await fetch(`${apiBaseUrl}/user/${userId}`, {
//                     headers: {
//                         'Authorization': `Bearer ${localStorage.getItem('jwtToken')}`
//                     }
//                 });

//                 if (!response.ok) {
//                     throw new Error('Failed to fetch user');
//                 }

//                 const user = await response.json();
//                 userDetails.innerHTML = `
//                     <p><strong>Username:</strong> ${user.username}</p>
//                     <p><strong>Role:</strong> ${user.role}</p>
//                 `;
//             } catch (error) {
//                 console.error('Error:', error);
//                 userDetails.innerHTML = '<p>Error fetching user</p>';
//             }
//         });
//     }

//     if (getAllFilmStudiosButton) {
//         getAllFilmStudiosButton.addEventListener('click', async () => {
//             try {
//                 const response = await fetch(`${apiBaseUrl}/filmstudio`, {
//                     headers: {
//                         'Authorization': `Bearer ${localStorage.getItem('jwtToken')}`
//                     }
//                 });

//                 if (!response.ok) {
//                     throw new Error('Failed to fetch film studios');
//                 }

//                 const filmStudios = await response.json();
//                 const filmStudiosArray = filmStudios.$values || filmStudios;
//                 if (!Array.isArray(filmStudiosArray)) {
//                     throw new Error('Film Studios response is not an array');
//                 }

//                 filmStudioList.innerHTML = '';

//                 if (filmStudiosArray.length === 0) {
//                     filmStudioList.innerHTML = '<p>No film studios found.</p>';
//                 } else {
//                     filmStudiosArray.forEach(studio => {
//                         const studioElement = document.createElement('div');
//                         studioElement.classList.add('studio');
//                         studioElement.innerHTML = `
//                             <p><strong>Username:</strong> ${studio.username}</p>
//                             <p><strong>Name:</strong> ${studio.name}</p>
//                             <p><strong>City:</strong> ${studio.city}</p>
//                             <p><strong>Email:</strong> ${studio.email}</p>
//                             <p><strong>Role:</strong> ${studio.role}</p>
//                         `;
//                         filmStudioList.appendChild(studioElement);
//                     });
//                 }
//             } catch (error) {
//                 console.error('Error:', error);
//                 filmStudioList.innerHTML = '<p>Error fetching film studios</p>';
//             }
//         });
//     }

//     if (getFilmStudioButton) {
//         getFilmStudioButton.addEventListener('click', async () => {
//             const studioId = filmStudioIdInput.value;
//             if (!studioId) {
//                 alert('Please enter a film studio ID');
//                 return;
//             }

//             try {
//                 const response = await fetch(`${apiBaseUrl}/filmstudio/${studioId}`, {
//                     headers: {
//                         'Authorization': `Bearer ${localStorage.getItem('jwtToken')}`
//                     }
//                 });

//                 if (!response.ok) {
//                     throw new Error('Failed to fetch film studio');
//                 }

//                 const studio = await response.json();
//                 filmStudioDetails.innerHTML = `
//                     <p><strong>Username:</strong> ${studio.username}</p>
//                     <p><strong>Name:</strong> ${studio.name}</p>
//                     <p><strong>City:</strong> ${studio.city}</p>
//                     <p><strong>Email:</strong> ${studio.email}</p>
//                     <p><strong>Role:</strong> ${studio.role}</p>
//                 `;
//             } catch (error) {
//                 console.error('Error:', error);
//                 filmStudioDetails.innerHTML = '<p>Error fetching film studio</p>';
//             }
//         });
//     }

//     function fetchAllFilms() {
//         fetch(`${apiBaseUrl}/film`, {
//             headers: {
//                 'Authorization': `Bearer ${localStorage.getItem('jwtToken')}`
//             }
//         })
//         .then(response => {
//             if (!response.ok) {
//                 throw new Error('Failed to fetch films');
//             }
//             return response.json();
//         })
//         .then(films => {
//             console.log('Films response:', films); // Log the response to debug
//             const filmsArray = films.$values || films;
//             if (!Array.isArray(filmsArray)) {
//                 throw new Error('Films response is not an array');
//             }

//             filmList.innerHTML = '';
//             if (filmsArray.length === 0) {
//                 filmList.innerHTML = '<p>No films available.</p>';
//             } else {
//                 filmsArray.forEach(film => {
//                     const filmElement = document.createElement('div');
//                     filmElement.classList.add('film');
//                     filmElement.innerHTML = `
//                         <h3>${film.title}</h3>
//                         <p>${film.description}</p>
//                         <p><strong>Director:</strong> ${film.director}</p>
//                         <p><strong>Year:</strong> ${film.year}</p>
//                         <p><strong>Genre:</strong> ${film.genre}</p>
//                         <p><strong>Available Copies:</strong> ${film.availableCopies}</p>
//                         ${isAdminPage ? `
//                         <button onclick="editFilm(${film.filmCopyId})">Edit</button>
//                         <button onclick="deleteFilm(${film.filmCopyId})">Delete</button>
//                         <form id="editFilmForm-${film.filmCopyId}" style="display: none;">
//                             <label for="title">Title:</label>
//                             <input type="text" id="title-${film.filmCopyId}" name="title" value="${film.title}" required>
//                             <label for="description">Description:</label>
//                             <input type="text" id="description-${film.filmCopyId}" name="description" value="${film.description}" required>
//                             <label for="availableCopies">Available Copies:</label>
//                             <input type="number" id="availableCopies-${film.filmCopyId}" name="availableCopies" value="${film.availableCopies}" required>
//                             <label for="genre">Genre:</label>
//                             <input type="text" id="genre-${film.filmCopyId}" name="genre" value="${film.genre}" required>
//                             <label for="director">Director:</label>
//                             <input type="text" id="director-${film.filmCopyId}" name="director" value="${film.director}" required>
//                             <label for="year">Year:</label>
//                             <input type="number" id="year-${film.filmCopyId}" name="year" value="${film.year}" required>
//                             <label for="isAvailable">Is Available:</label>
//                             <input type="checkbox" id="isAvailable-${film.filmCopyId}" name="isAvailable" ${film.isAvailable ? 'checked' : ''}>
//                             <button type="submit">Save</button>
//                         </form>` : ''}
//                     `;
//                     filmList.appendChild(filmElement);

//                     if (isAdminPage) {
//                         document.getElementById(`editFilmForm-${film.filmCopyId}`).addEventListener('submit', async (event) => {
//                             event.preventDefault();
//                             const updatedFilmData = {
//                                 title: document.getElementById(`title-${film.filmCopyId}`).value,
//                                 description: document.getElementById(`description-${film.filmCopyId}`).value,
//                                 availableCopies: parseInt(document.getElementById(`availableCopies-${film.filmCopyId}`).value),
//                                 genre: document.getElementById(`genre-${film.filmCopyId}`).value,
//                                 director: document.getElementById(`director-${film.filmCopyId}`).value,
//                                 year: parseInt(document.getElementById(`year-${film.filmCopyId}`).value),
//                                 isAvailable: document.getElementById(`isAvailable-${film.filmCopyId}`).checked
//                             };

//                             try {
//                                 const response = await fetch(`${apiBaseUrl}/film/${film.filmCopyId}`, {
//                                     method: 'PATCH',
//                                     headers: {
//                                         'Content-Type': 'application/json',
//                                         'Authorization': `Bearer ${localStorage.getItem('jwtToken')}`
//                                     },
//                                     body: JSON.stringify(updatedFilmData)
//                                 });

//                                 if (!response.ok) {
//                                     throw new Error('Failed to update film');
//                                 }

//                                 alert('Film updated successfully');
//                                 fetchAllFilms();
//                             } catch (error) {
//                                 console.error('Error:', error);
//                                 alert(`Error updating film: ${error.message}`);
//                             }
//                         });
//                     }
//                 });
//             }
//         })
//         .catch(error => {
//             console.error('Error:', error);
//             filmList.innerHTML = `<p>Error fetching films: ${error.message}</p>`;
//         });
//     }

//     async function editFilm(filmCopyId) {
//         const editForm = document.getElementById(`editFilmForm-${filmCopyId}`);
//         editForm.style.display = editForm.style.display === 'none' ? 'block' : 'none';
//     }

//     async function deleteFilm(filmCopyId) {
//         if (confirm('Are you sure you want to delete this film?')) {
//             try {
//                 const response = await fetch(`${apiBaseUrl}/film/${filmCopyId}`, {
//                     method: 'DELETE',
//                     headers: {
//                         'Authorization': `Bearer ${localStorage.getItem('jwtToken')}`
//                     }
//                 });

//                 if (!response.ok) {
//                     throw new Error('Failed to delete film');
//                 }

//                 alert('Film deleted successfully');
//                 fetchAllFilms();
//             } catch (error) {
//                 console.error('Error:', error);
//                 alert(`Error deleting film: ${error.message}`);
//             }
//         }
//     }
//     // Fetch all films on page load
//     fetchAllFilms();
// });

// function logout() {
//     localStorage.removeItem('jwtToken');
//     localStorage.removeItem('role');
//     window.location.href = 'index.html';
// }