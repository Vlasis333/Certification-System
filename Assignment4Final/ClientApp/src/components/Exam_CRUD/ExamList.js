import React, { useEffect, useState, useContext } from "react";
import axios from 'axios';
import { ListGroup, ListGroupItem, Button, Table, Row, Stack, Form } from 'react-bootstrap';
import { useNavigate, Link } from "react-router-dom";
import { AuthenticationContext } from '../auth/AuthenticationContext'
import Multiselect from 'multiselect-react-dropdown';
import BackButton from "../Common/Back";
import { trackPromise } from "react-promise-tracker";
import LoadingIndicator from "../Common/LoadingIndicator";

function ExamList(props) {
    const [exams, setExams] = useState([]);
    const [showForm, setShowForm] = useState(false);
    const [certificates, setCertificates] = useState([]);
    const [pickedCert, setPickedCert] = useState()
    const [pickedTopics, setPickedtopics] = useState([])
    const [certId, setCertId] = useState();
    const [createdExam, setCreatedExam] = useState({})
    let navigate = useNavigate();
    const { update, claims } = useContext(AuthenticationContext);
    const role = claims.find(claim => claim.name === 'role').value
    // const [user, setUser] = useState();
    // let navigate = useNavigate();

    useEffect(() => {
        trackPromise(axios.get('https://localhost:7196/api/Exam').then((response) => {
            setExams(response.data)
        }).catch(function(error) {
            console.log(error)
        }))
    }, []);


    const getCertificates = async () => {
        {
            await axios.get(`https://localhost:7196/api/Certificates`).then((response) => {
                setCertificates(response.data.data)
            })
        }
    }

    const handleDelete = (examId) => {
        const confirmDelete = window.confirm("Are you sure you want to delete this exam?");
        setExams(exams.filter(exam => exam.id !== examId));
        if (confirmDelete) {

            axios.delete(`https://localhost:7196/api/Exam/${examId}`)
                .then(response => {
                    console.log(response)
                })
        }
    }


    const handleEdit = (examEdit) => {
        navigate(`/ExamQuestionList/${examEdit.id}`)
    }

    const makeButtons = (examButtons) => {
        if (role === "admin") {
            return (
                <div className='d-flex gap-2'>
                    <Button onClick={() => handleEdit(examButtons)} variant="outline-primary">Edit</Button>
                    <Button onClick={() => handleDelete(examButtons.id)} variant="outline-danger">Delete</Button>
                </div>
            )
        }
        if (role === "qualitycontrol") {
            return (
                <div>
                    <Button onClick={() => handleEdit(examButtons)} variant="primary">Details</Button>
                </div>
            )
        }
    }

    const calculateCount = (questionsArray) => {
        return (<>{questionsArray.length}</>)
    }

    const changeStates = (cId) => {
        console.log("id in state", certId)
        var certiff = certificates.find(certificate => certificate.id == certId)
        setPickedCert(certiff)
    }

    useEffect(() => {
        console.log('piced cert THISS', certId)
    }, [pickedCert]
    )

    const handleChange = function(event) {
        var id = event.target.value
        var name = event.target.name

        console.log("------------id", id)
        console.log("------------name", name)
        console.log('certId', id)

        setCreatedExam({ ...createdExam, [name]: certificates.find(certificate => certificate.id == id), id: 0 })
        console.log(createdExam)
    }

    const handleSubmitExam = async event => {
        event.preventDefault();
        var certif = certificates.find(certificate => certificate.id == certId)
        console.log('LOOK HEREE', createdExam)
        await axios.post(`https://localhost:7196/api/Exam`, createdExam).then((response) => {
            console.log(response)
            navigate(`/AddQuestionToExam/${response.data.data.id}`)
        })
    }

    const onRemove = (selectedTopics, removedTopic) => {
        createdExam.topics = selectedTopics.filter(item => item !== removedTopic)
        setCreatedExam({ createdExam })
    }

    const createCertificateButton = async () => {
        await getCertificates();
        setShowForm(!showForm)
    }

    return (
        <div>
            <h1 class="display-3 text-center align-middle">Exams</h1>
            {role === "admin" &&
                <Button className='d-grid gap-2 col-6 mx-auto py-2 my-2'
                    onClick={() => createCertificateButton()} variant="success">
                    {showForm ? "Close Form" : "Create New Exam"}
                </Button>}
            {showForm && (
                <Form onSubmit={handleSubmitExam}>
                    <Form.Select as="select" name="certificate"
                        onChange={handleChange} required>
                        <option value="" hidden>Please choose a certificate... </option>
                        {certificates.map((certificate, index) =>
                            <option key={index} value={certificate.id}>{certificate.title}</option>
                        )}
                    </Form.Select>
                    <Button type="submit">"Save Exam & Add Answers"</Button>
                </Form>
            )}
            <Table striped borderless hover id='list_Of_Exams'>
                <thead>
                    <th>Certificate Title</th>
                    <th>Number Of Questions</th>
                </thead>
                <tbody>
                    {exams.map((exam, index) => {
                        if (exam.certificate != null && exam != null) {
                            return (
                                <tr key={index}>
                                    <td>{exam.certificate.title}</td>
                                    <td>{calculateCount(exam.questions)}</td>
                                    <td>{makeButtons(exam)}</td>
                                </tr>
                            )
                        }
                    })}
                </tbody>
            </Table>
            <LoadingIndicator />
            <Button variant='secondary' className='d-grid gap-2 col-12 mx-auto py-2 my-2' onClick={() => navigate("/")} >Go Back Home Page</Button>
        </div>
    )

}
export default ExamList
