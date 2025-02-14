export function registerUser(apiBaseUrl) {
    const registerForm = document.getElementById('registerForm');
    const registerMessage = document.getElementById('registerMessage');

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
                window.location.href = 'login.html';
            } catch (error) {
                console.error('Error:', error);
                registerMessage.textContent = 'Error registering: ' + error.message;
            }
        });
    }
}

export function loginUser(apiBaseUrl) {
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
                localStorage.setItem('jwtToken', data.token);
                localStorage.setItem('role', data.role);

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
}

export function fetchAllUsers(apiBaseUrl) {
    const getAllUsersButton = document.getElementById('getAllUsersButton');
    const userList = document.getElementById('userList');

    if (getAllUsersButton) {
        getAllUsersButton.addEventListener('click', async () => {
            try {
                const response = await fetch(`${apiBaseUrl}/user/all`, {
                    headers: {
                        'Authorization': `Bearer ${localStorage.getItem('jwtToken')}`
                    }
                });

                if (!response.ok) {
                    throw new Error('Failed to fetch users');
                }

                const users = await response.json();
                const usersArray = users.$values || users;
                if (!Array.isArray(usersArray)) {
                    throw new Error('Users response is not an array');
                }

                userList.innerHTML = '';

                if (usersArray.length === 0) {
                    userList.innerHTML = '<p>No users found.</p>';
                } else {
                    usersArray.forEach(user => {
                        const userElement = document.createElement('div');
                        userElement.classList.add('user');
                        userElement.innerHTML = `
                            <p><strong>Username:</strong> ${user.username}</p>
                            <p><strong>Role:</strong> ${user.role}</p>
                        `;
                        userList.appendChild(userElement);
                    });
                }
            } catch (error) {
                console.error('Error:', error);
                userList.innerHTML = '<p>Error fetching users</p>';
            }
        });
    }
}

export function getUser(apiBaseUrl) {
    const getUserButton = document.getElementById('getUserButton');
    const userIdInput = document.getElementById('userIdInput');
    const userDetails = document.getElementById('userDetails');

    if (getUserButton) {
        getUserButton.addEventListener('click', async () => {
            const userId = userIdInput.value;
            if (!userId) {
                alert('Please enter a user ID');
                return;
            }

            try {
                const response = await fetch(`${apiBaseUrl}/user/${userId}`, {
                    headers: {
                        'Authorization': `Bearer ${localStorage.getItem('jwtToken')}`
                    }
                });

                if (!response.ok) {
                    throw new Error('Failed to fetch user');
                }

                const user = await response.json();
                userDetails.innerHTML = `
                    <p><strong>Username:</strong> ${user.username}</p>
                    <p><strong>Role:</strong> ${user.role}</p>
                `;
            } catch (error) {
                console.error('Error:', error);
                userDetails.innerHTML = '<p>Error fetching user</p>';
            }
        });
    }
}