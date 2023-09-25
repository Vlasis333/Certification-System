import { Form, FormGroup, Button, Col, Row, Badge, Stack } from 'react-bootstrap';
import { Link, useNavigate } from "react-router-dom";
import { React, useState, useEffect,useContext } from 'react';

import axios from 'axios';
import { useLocation, useParams } from 'react-router-dom';
import { AuthenticationContext } from '../auth/AuthenticationContext'


import MyEditor from "./Editor";
import Errors from '../Common/ErrorList'

import BackButton from "../Common/Back";
//--------------------------------------------------QUESTION EDIT FUNCTION--------------------------------------------------
export default function QuestionEdit(event, name) {

    const numbers = ['First','Second','Third','Fourth'];

    const params = useParams();
    const navigate = useNavigate();
    const [error, setError] = useState(null);
    const [data, setData] = useState([]);
    const { update, claims } = useContext(AuthenticationContext);

    const [role, setRole] = useState(claims.find(claim => claim.name === 'role').value)

    //------------------------------------------------Question state
    const [question, setQuestion] = useState({
        text: "string",
        topicId: 0,
        difficultyLevelId: 0,

        options: [],
    });

    const [options, setOptions] = useState([{}, {}, {}, {}]);

    //------------------------------------------------Topics state
    const [allTopics, setAllTopics] = useState([
        { id: 0, name: "string", questions: [] },
    ]);
    //------------------------------------------------Difficulty levels state
    const [difficultyLevels, setLevels] = useState([]);

    const location = useLocation();
    const QuestionId = params.id;
    //Same outcome as above------------------------------------------------
    // const _params = useParams();
    // const params = _params.id;
    //--------------------------------------------------

    //------------------------------------------------//GET ALL TOPICS AND DIFFICULTY LEVELS
    // const url = "https://localhost:7196/api/Questions/" + questionIndex;
    const url = `https://localhost:7196/api/Questions/${QuestionId}`;
    useEffect(() => {
        axios.get(`https://localhost:7196/api/Topics`)
            .then((res) => {
                setAllTopics(res.data.data);
            })
            .catch((err) => {
                console.error(err.response.data);
            });

        axios.get(`https://localhost:7196/api/DifficultyLevels`)
            .then((res) => {
                setLevels(res.data.data);

            })
            .catch((err) => {
                console.error(err.response.data);
            });
        //------------------------------------------------//GET QUESTION BY ID
       axios.get(url).then((response) => {
            setQuestion(response.data.data);
            setOptions(response.data.data.options);
        });
    }, [url]);//------------------------------------------------//GET QUESTION BY ID

    console.log(question);
    //------------------------------------------------//HANDLE SUBMIT
    const handleSubmit = (event) => {
        event.preventDefault();
        // console.log(question);
        const newQuestion = { ...question, options: options };
        console.log(newQuestion);
        setQuestion(newQuestion);
        console.log(" Put!!!");

        axios.put(url, newQuestion)
            .then((response) => {
                console.log("Done!!!");
                setError([]);
                navigate("/questions")
            }).catch((err) => {
                console.error(err);
                console.error(err.response);
                console.error(err.response.data);
                console.error(err.response.data.message);
                setError(err);
            });
    };
    //------------------------------------------------HANDLE CHANGE------------------------------------------------>>>>>>------------------------------------   
    // event, name, data, optionId
    const handleChange = (event, name, data, questionOptionIndex) => {
        let newQuestion;
        let newOptions = [];
        const checkBoxKey = name;
        console.log("In handleChange");
        //Condition which  assures that setQuestion is called only by the user's input------------------------->>>>>>
        if (question.text !== "string") {
            if (name === "QuestionText") {
                console.log(data);
                newQuestion = { ...question, text: data };
                setQuestion(newQuestion);
            } else if (name === "OptionText") {
                newOptions = options.map((option, index) =>
                    index === questionOptionIndex
                        ? { ...option, text: data }
                        : option
                );
                setOptions(newOptions);
            }
            else if (checkBoxKey === "checkbox") {
                console.log("Trying to change the correct option");
                console.log(!data);
                setOptions(
                    options.map((option, index) =>
                        index === questionOptionIndex
                            ? { ...option, correct: !data }
                            : option
                    )
                );
                console.log(options);
            }
            else if (name === "TopicSelect") {
                console.log("Trying to change the topic");
                console.log(event);
                console.log(name);
                console.log(data);
                // console.log(data);
                // setQuestion({ ...question, topicId: data });
            }
        }
    };
    //------------------------------------------------HANDLE SELECT------------------------------------------------>>>>>>
    const onSelect = (event, selectedItem, species) => {
        console.log(question.difficultyLevelId);
        console.log("IN SELECT");
        if (species === "topic") {
            const newQuestion = { ...question, topicId: selectedItem.id, topic: selectedItem };
            setQuestion(newQuestion);
        }
        else if (species === "difficultyLevel") {

            const newQuestion = { ...question, difficultyLevelId: selectedItem.id, difficultyLevel: selectedItem };

            setQuestion(newQuestion);
        }
    };
    //------------------------------------------------//----------------------->>>>>>>

    return (
        //---||FORM------------------------------------------------->>>>>>>
        <div>
            {
                role && role !== "qualitycontrol" ?
                <h1 class="display-3 text-center align-middle">Edit Question</h1> :
                <h1 class="display-3 text-center align-middle">Question Details</h1>
            }
            {error && <Errors error={error} />}
            <fieldset disabled={role ? (role === "qualitycontrol") : false}>
            <Form noValidate validated={true} onSubmit={handleSubmit}>
                {/* DROPDOWN TOPICS */}
                <Form.Group className='mb-2'>
                    <h4>Question's Topic</h4>

                    <Form.Select
                        as="select"
                        name="TopicSelect"
                        value={question.topicId}                
                    // onChange={ handleChange  }     
                                                                             
                    //  required                                   
                    >
                        <option value="" hidden>
                            Please choose a topic{" "}
                        </option>
                        {allTopics.map((topic, index) => (
                            <option
                                key={index}
                                onClick={() => {
                                    onSelect(event, topic, "topic");
                                }}
                                value={topic.id}
                                onChange={console.log("Topic changed")}
                            >
                                {topic.name}
                            </option>
                        ))}
                    </Form.Select>
                </Form.Group>
              
                    <Form.Group>
                                {/*  DROPDOWN Difficulty levels */}
                        <h4> Question's Difficulty</h4>
                        <Form.Select
                            className='w-100'
                            as="select"
                            name="DifficultySelect"
                            value={question.difficultyLevelId}  
                        onChange={ console.log("Difficulty changed")  }     
                        //  required                                     
                        >
                            <option value={0} hidden>
                                Please choose a level{" "}
                            </option>
                            {difficultyLevels.map((difficultyLevel, index) => (
                                <option
                                    key={index}
                                    onClick={() => {
                                        onSelect(event, difficultyLevel, "difficultyLevel");
                                    }}  
                                    value={difficultyLevel.id}
                                    onChange={console.log("Difficulty changed")}
                                >
                                    {difficultyLevel.difficulty}
                                </option>
                            ))}
                        </Form.Select>
                    </Form.Group>
              
                <hr />

                <Stack gap={2}>
                    <Row key={"questionEditorAndDropdowns"}>
                        {/*-------------Questions text */}
                        <Col >
                            <FormGroup required>
                                <Form.Label><h4>Questions Text</h4></Form.Label>

                                <MyEditor
                                    key={"questionEditor"}
                                    handleChange={handleChange}
                                    name={"QuestionText"}
                                    text={question.text}
                                />

                            </FormGroup>
                        </Col>
                    </Row>
                    {/*   Question OPTIONS */}
                    {options.map((option, index) => (
                        <Row key={"unique" + index} className="align-items-center justify-content-center">
                            <Form.Label><h5>{numbers[index]} Option </h5></Form.Label>
                            <Col >
                                    <MyEditor
                                        key="index"
                                        handleChange={handleChange}
                                        name={"OptionText"}
                                        text={option.text}
                                        index={index}
                                    />
                            </Col>
                            <Col xs={2}>
                                <Form.Check
                                    type={"checkbox"}
                                // id={index}
                                // label={"Is Correct"}
                                // onChange={handleChange}
                                //    onClick={(event) => handleChange(event)}
                                // name={"checkbox "}
                                >
                                    <Form.Check.Label>
                                         Is Correct 
                                    </Form.Check.Label>
                                    <Form.Check.Input
                                        type={"checkbox"}
                                        isValid
                                        checked={option.correct}
                                        onChange={() => {
                                            handleChange(
                                                event,
                                                "checkbox",
                                                option.correct,
                                                index
                                            );
                                        }}
                                        name="checkbox"
                                        id={index.toString()}
                                    />
                                </Form.Check>
                            </Col>
                        </Row>
                    ))}
                   

                        <div className='w-100 mb-3 '>
                            <Button variant="primary" type="submit" value={"Submit"} className="w-100 mb-3" >
                                Update Question
                            </Button>
                          

                        </div>
                        
                 
                </Stack>
            </Form>
            </fieldset>
                        <BackButton />

        </div>
    );
}


