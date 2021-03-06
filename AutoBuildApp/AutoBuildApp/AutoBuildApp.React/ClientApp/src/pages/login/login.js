import React from 'react';
import Modal from 'react-bootstrap/Modal';
import "./login.css";

/* 
Login modal:
  Fields:
    email/username
    password
  Buttons:
    Login
    Forgot Password
    Create Account
*/
function Login(props) {
  return(
    <Modal
      {...props}
      size="lg"
      aria-labelledby="contained-login-modal"
      centered
    >
        <Modal.Header closeButton/>

        <Modal.Body>
            <h4>Login</h4>
            {/* Place form here */}
        </Modal.Body>
    </Modal>
  );
}

export default Login;