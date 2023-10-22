document.getElementById("loginForm").addEventListener("submit", function (event) {
    event.preventDefault(); // Prevent the form from submitting

    const email = document.getElementById("email").value;
    const password = document.getElementById("password").value;

    const data = {
        Email: email,
        Password: password
    };

    fetch(this.action, {
        method: "POST",
        headers: {
            "Content-Type": "application/json"
        },
        body: JSON.stringify(data)
    })
    .then(response => response.json())
    .then(responseData => {
        if (responseData.token) {
            // Successfully logged in, you can handle the token as needed
            // For example, you can store it in localStorage for authentication
            localStorage.setItem("token", responseData.token);
            // Redirect the user to a dashboard or another page
            window.location.href = "dashboard.html";
        } else {
            // Handle login failure, show an error message or handle it as needed
            alert("Login failed. Please check your email and password.");
        }
    })
    .catch(error => {
        console.error("API request failed: " + error);
    });
});