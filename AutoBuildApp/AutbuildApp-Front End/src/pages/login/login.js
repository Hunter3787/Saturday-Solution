import Modal from 'react-bootstrap/Modal';

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