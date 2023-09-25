import React, { useState } from "react";
import { Button, Alert } from 'react-bootstrap';

const ErrorsRegister = ({ error }) => {

    const [show, setShow] = useState(true);

    const makeErrors = (error) => {
        if (Array.isArray(error.response.data)) {
            return error.response.data.map(object => <li>{object.description}</li>)
        }
        else {
            return Object.entries(error.response.data.errors).map(([key, value]) => (
                <li key={key}>{key}: {value[0]}</li>
            ))
        }
    }
    // error.response.data.errors
    return error && error.response && error.response.data  ? (
        <>
            <Button variant="danger" onClick={() => setShow(!show)} style={{ marginBottom: '10px' }}>
                {show ? 'Minimize' : 'Show'}
            </Button>

            {show && (
                <Alert variant="danger">
                    <ul className="my-auto">
                        {
                            makeErrors(error)
                        }
                    </ul>
                </Alert>
            )}
        </>
    ) : error ? (
        <>
            <Button variant="danger" onClick={() => setShow(!show)} style={{ marginBottom: '10px' }}>
                {show ? 'Minimize' : ' Show'}
            </Button>

            {show && (
                <Alert variant="danger">
                    <p>{error}</p>
                </Alert>
            )}
        </>
    ) : null;
};

export default ErrorsRegister;
