import React, { useEffect, useState, useContext } from "react";
import { useNavigate } from "react-router-dom";
import { AuthenticationContext } from "../auth/AuthenticationContext";
import { Button, Table } from "react-bootstrap";
import axios from "axios";
import BackButton from "../Common/Back";
import { trackPromise } from "react-promise-tracker";
import LoadingIndicator from "../Common/LoadingIndicator";

function UsersList() {
  //const [buttons, setButtons] = useState();
  const [users, setUsers] = useState([]);
  const [roles, setRoles] = useState();
  let navigate = useNavigate();
  const { update, claims } = useContext(AuthenticationContext);
  const [role, setRole] = useState(
    claims.find((claim) => claim.name === "role").value
  );

  useEffect(() => {
    // console.log(claims.findIndex(claim => claim.name === 'role' && claim.value === 'candidate'))
    // console.log(claims.findIndex(claim => claim.name === 'role' && claim.value === 'admin'))
    console.log(update);
    console.log(claims);
    console.log(role);

    trackPromise(
      axios
        .get("https://localhost:7196/api/accounts/listUsers")
        .then((response) => {
          setUsers(response.data);
          console.log(response.data);
          // console.log("hey")
        })
        .catch(function (error) {
          console.log(error);
        })
    );

    // axios.get('https://localhost:7196/api/accounts/getAllClaims').then((response) => {
    //     setRoles(response.data);
    //     console.log(response.data)
    //     // console.log("hey")
    // }).catch(function (error) {
    //     console.log(error);
    // });
  }, []);

  const handleDelete = async (userEmail) => {
    console.log("delete for id  = ", userEmail);
    const confirmDelete = window.confirm(
      "Are you sure you want to delete this User?"
    );
    if (confirmDelete) {
      await axios
        .delete(`https://localhost:7196/api/accounts/delete/${userEmail}`)
        .then((response) => {
          console.log(response);
          setUsers((prevData) =>
            prevData.filter((item) => item.email !== userEmail)
          );
          // setData(prevData => prevData.filter(item => item.appUserId !== candId));
        })
        .catch((response) => {
          console.log(response);
        });
    }
  };

  const handleEdit = (userEmail) => {
    //console.log("edit for id  = ", candId);
    navigate(`/users/edit/${userEmail}`);
  };

  return (
    <div>
      <h1 class="display-3 text-center align-middle">Users</h1>
      <Button
        variant="dark"
        className="d-grid gap-2 col-6 mx-auto py-2 my-2"
        onClick={() => navigate("/users/create")}
      >
        {" "}
        Add a User{" "}
      </Button>

      <Table
        striped
        hover
        borderless
        className="text-center"
        id="list_of_allcands"
      >
        <thead>
          <tr>
            <th>Username</th>
            <th>Email</th>
            <th>Role</th>
            <th>Phone Number</th>
            <th>Actions</th>
          </tr>
        </thead>
        <tbody>
          {users.map((user, index) => (
            <tr key={index}>
              <td>{user.userName}</td>
              <td>{user.email}</td>
              <td>{user.role}</td>
              <td>{user.phoneNumber}</td>
              <td className="d-flex justify-content-center">
                <div className="d-flex gap-2">
                  <Button onClick={() => handleEdit(user.email)}>Edit</Button>
                  <Button
                    variant="dark"
                    onClick={() => handleDelete(user.email)}
                  >
                    Delete
                  </Button>
                </div>
              </td>
            </tr>
          ))}
        </tbody>
      </Table>
      <LoadingIndicator />
      <BackButton />
    </div>
  );
}
export default UsersList;
