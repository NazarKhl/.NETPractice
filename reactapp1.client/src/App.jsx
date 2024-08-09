import { useState, useEffect } from 'react';
import './App.css';
import { Modal, Button, Input, Checkbox, notification, DatePicker, Select } from 'antd';
import moment from 'moment';
import UserChart from './Charts/UserChart';

const { RangePicker } = DatePicker;
const { Option } = Select;

export default function App() {
    const [users, setUsers] = useState([]);
    const [userFinder, setUserFinder] = useState('');
    const [selectedUser, setSelectedUser] = useState(null);
    const [loading, setLoading] = useState(false);
    const [isCreateModalOpen, setIsCreateModalOpen] = useState(false);
    const [isUpdateModalOpen, setIsUpdateModalOpen] = useState(false);
    const [isAbsenceModalOpen, setIsAbsenceModalOpen] = useState(false);
    const [isShowAbsencesModalOpen, setIsShowAbsencesModalOpen] = useState(false);
    const [isUserActive, setIsUserActive] = useState(false);
    const [absenceType, setAbsenceType] = useState(null);
    const [absenceDescription, setAbsenceDescription] = useState('');
    const [absenceDateFrom, setAbsenceDateFrom] = useState(null);
    const [absenceDateTo, setAbsenceDateTo] = useState(null);

    useEffect(() => {
        showUsers();
        
    }, []);

    const showUsers = async () => {
        setLoading(true);
        try {
            const response = await fetch('/api/user');
            const data = await response.json();
            setUsers(data);
        } catch (error) {
            notification.error({ description: error.message });
        } finally {
            setLoading(false);
            setUserFinder('');
        }
    };

    const handleInput = (e) => {
        setUserFinder(e.target.value);
    };

    const showCreateModal = () => {
        setSelectedUser({ name: '', email: '', isActive: false, absences: [] });
        setIsCreateModalOpen(true);
    };

    const showUpdateModal = (user) => {
        setSelectedUser(user);
        setIsUserActive(user?.isActive || false);
        setIsUpdateModalOpen(true);
    };

    const showAbsenceModal = (user) => {
        setSelectedUser(user);
        setAbsenceType(null);
        setAbsenceDescription('');
        setAbsenceDateFrom(null);
        setAbsenceDateTo(null);
        setIsAbsenceModalOpen(true);
    };

    const showUserAbsences = async (user) => {
        setLoading(true);
        try {
            const response = await fetch(`/api/Absence`);
            const absences = await response.json();
            setSelectedUser({ ...user, absences });
            setIsShowAbsencesModalOpen(true);
        } catch (error) {
            notification.error({ description: error.message });
        } finally {
            setLoading(false);
        }
    };


    const hideModals = () => {
        setIsCreateModalOpen(false);
        setIsUpdateModalOpen(false);
        setIsAbsenceModalOpen(false);
        setIsShowAbsencesModalOpen(false);
        setSelectedUser(null);
    };

    const handleCreateUser = async () => {
        try {
            const payload = { ...selectedUser, isActive: isUserActive };
            await fetch('/api/user', {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify(payload),
            });
            notification.success({ message: 'User created successfully' });
            await showUsers();
            hideModals();
        } catch (error) {
            notification.error({ message: 'Error creating user', description: error.message });
        }
    };

    const handleUpdateUser = async () => {
        try {
            const payload = { ...selectedUser, isActive: isUserActive };
            await fetch(`/api/user/${selectedUser.id}`, {
                method: 'PUT',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify(payload),
            });
            notification.success({ message: 'User updated successfully' });
            await showUsers();
            hideModals();
        } catch (error) {
            notification.error({ message: 'Error updating user', description: error.message });
        }
    };

    const handleAbsenceAdd = async () => {
        const newAbsence = {
            userId: selectedUser?.id,
            type: absenceType,
            description: absenceDescription,
            dateFrom: absenceDateFrom ? absenceDateFrom.toISOString() : null,
            dateTo: absenceDateTo ? absenceDateTo.toISOString() : null,
        };

        try {
            await fetch('/api/absence', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                },
                body: JSON.stringify(newAbsence),
            });
            notification.success({ message: 'Absence added successfully' });
            await showUsers();
            hideModals();
        } catch (error) {
            notification.error({ message: 'Error adding absence', description: error.message });
        } finally {
            setAbsenceType(null);
            setAbsenceDescription('');
            setAbsenceDateFrom(null);
            setAbsenceDateTo(null);
        }
    };

    const handleDelete = (userId) => {
        Modal.confirm({
            title: 'Are you sure you want to delete this user?',
            okText: 'Yes',
            cancelText: 'No',
            onOk: async () => {
                try {
                    await fetch(`/api/user/${userId}`, {
                        method: 'DELETE',
                    });
                    notification.success({ message: 'User deleted successfully' });
                    await showUsers();
                } catch (error) {
                    notification.error({ message: 'Error deleting user', description: error.message });
                }
            },
        });
    };

    const handleInputChange = (e) => {
        setSelectedUser(prev => ({ ...prev, [e.target.name]: e.target.value }));
    };

    const removeAbsence = async (index) => {
        const absenceId = selectedUser.absences[index].id;
        try {
            await fetch(`/api/absence/${absenceId}`, {
                method: 'DELETE',
            });
            notification.success({ message: 'Absence removed successfully' });
            await showUsers();
        } catch (error) {
            notification.error({ message: 'Error removing absence', description: error.message });
        }
    };

    const downloadJSON = () => {
        const blob = new Blob([JSON.stringify(users, null, 2)], { type: 'application/json' });
        const url = URL.createObjectURL(blob);
        const link = document.createElement('a');
        link.href = url;
        link.download = 'users.json';
        link.click();
        URL.revokeObjectURL(url);
    };

    const handleUserActivityChange = (e) => {
        setIsUserActive(e.target.checked);
    };


    const filteredUsers = userFinder
        ? users.filter(user => user.id === parseInt(userFinder, 10))
        : users;

    const absenceTypeLabels = {
        1: 'Illness',
        2: 'Vacation',
        3: 'Other'
    };

    const contents = loading
        ? <p><em>Loading... Please wait.</em></p>
        : filteredUsers.length === 0
            ? <p><em>No users found. Click "Show all users" to fetch data.</em></p>
            : <table className="table table-striped" aria-labelledby="tableLabel">
                <thead>
                    <tr>
                        <th>Id</th>
                        <th>Name</th>
                        <th>Email</th>
                        <th>Action buttons</th>
                    </tr>
                </thead>
                <tbody>
                    {filteredUsers.map(user =>
                        <tr key={user.id}>
                            <td>{user.id}</td>
                            <td>{user.name}</td>
                            <td>{user.email}</td>
                            <td>
                                <Button className="addAbsenceButton" onClick={() => showAbsenceModal(user)}>Add Absence</Button>
                                <Button className="updateButton" onClick={() => showUpdateModal(user)}>Update</Button>
                                <Button className="showAbsencesButton" onClick={() => showUserAbsences(user)}>Show Absences</Button>
                                <Button type="primary" className="deleteButton" onClick={() => handleDelete(user.id)} danger>Delete</Button>
                            </td>
                        </tr>
                    )}
                </tbody>
            </table>;

    return (
        <div>
            <h1 id="tableLabel">User Data</h1>
            <Input
                min={1}
                value={userFinder}
                onChange={handleInput}
                type="number"
                className="inputField"
                placeholder='Find by ID'
            />
            <Button onClick={showUsers} className="showUsers">Show all users</Button>
            <Button onClick={showCreateModal} className="createUser">Create New User</Button>
            <Button className="downloadJSON" onClick={downloadJSON}>Download .json</Button>
            <p>This component demonstrates fetching data from the server.</p>
            {contents}
            <UserChart users={users} />

            <Modal
                title="Create User"
                visible={isCreateModalOpen}
                onOk={handleCreateUser}
                onCancel={hideModals}
            >
                <Input
                    name="name"
                    value={selectedUser?.name || ''}
                    onChange={handleInputChange}
                    placeholder="Name"
                />
                <Input
                    name="email"
                    value={selectedUser?.email || ''}
                    onChange={handleInputChange}
                    placeholder="Email"
                />
                <div className="activeUsers">
                    <Checkbox checked={isUserActive} onChange={handleUserActivityChange} />
                    <label> User <strong>{selectedUser?.name}</strong> is {isUserActive ? 'active' : 'inactive'} </label>
                </div>
            </Modal>

            <Modal
                title="Update User"
                visible={isUpdateModalOpen}
                onOk={handleUpdateUser}
                onCancel={hideModals}
            >
                <Input
                    name="name"
                    value={selectedUser?.name || ''}
                    onChange={handleInputChange}
                    placeholder="Name"
                />
                <Input
                    style="inputField"
                    name="email"
                    value={selectedUser?.email || ''}
                    onChange={handleInputChange}
                    placeholder="Email"
                />
                <div className="activeUsers">
                    <Checkbox checked={isUserActive} onChange={handleUserActivityChange} />
                    <label> User <strong>{selectedUser?.name}</strong> is {isUserActive ? 'active' : 'inactive'} </label>
                </div>
                <ul>

                    {selectedUser?.absences.map((absence, index) => (
                        <li key={index}>
                            {absenceTypeLabels[absence.type]} - {absence.description} from {moment(absence.dateFrom).format('YYYY-MM-DD')} to {moment(absence.dateTo).format('YYYY-MM-DD')}
                            <Button onClick={() => removeAbsence(index)} type="link" danger style={{ marginLeft: '10px' }}>
                                Remove
                            </Button>
                        </li>
                    ))}
                </ul>
            </Modal>
            <Modal
                title="Add Absence"
                visible={isAbsenceModalOpen}
                onOk={handleAbsenceAdd}
                onCancel={hideModals}
            >
                <Select
                    value={absenceType}
                    onChange={value => setAbsenceType(value)}
                    placeholder="Select Absence Type"
                    style={{ width: '100%' }}
                >
                    <Option value={1}>Illness</Option>
                    <Option value={2}>Vacation</Option>
                    <Option value={3}>Other</Option>
                </Select>
                <Input
                    value={absenceDescription}
                    onChange={e => setAbsenceDescription(e.target.value)}
                    placeholder="Description"
                    style={{ marginTop: '10px' }}
                />
                <RangePicker
                    value={[absenceDateFrom ? moment(absenceDateFrom) : null, absenceDateTo ? moment(absenceDateTo) : null]}
                    onChange={(dates) => {
                        setAbsenceDateFrom(dates ? dates[0].toDate() : null);
                        setAbsenceDateTo(dates ? dates[1].toDate() : null);
                    }}
                    style={{ marginTop: '10px', width: '100%' }}
                />
            </Modal>

            <Modal
                title="User Absences"
                visible={isShowAbsencesModalOpen}
                onCancel={hideModals}
                footer={null}
            >
                <ul>
                    {selectedUser?.absences.map((absence, index) => (
                        <li key={index}>
                            <Select
                                value={absenceType !== null ? absenceType : absence.type}
                                onChange={value => setAbsenceType(value)}
                                placeholder="Select Absence Type"
                                style={{ width: '100%' }}
                            >
                                <Option value={1}>Illness</Option>
                                <Option value={2}>Vacation</Option>
                                <Option value={3}>Other</Option>
                            </Select>
                            <Input
                                value={absenceDescription || absence.description}
                                onChange={e => setAbsenceDescription(e.target.value)}
                                placeholder="Description"
                                style={{ marginTop: '10px' }}
                            />
                            <RangePicker
                                value={[
                                    absenceDateFrom ? moment(absenceDateFrom) : moment(absence.dateFrom),
                                    absenceDateTo ? moment(absenceDateTo) : moment(absence.dateTo)
                                ]}
                                onChange={(dates) => {
                                    setAbsenceDateFrom(dates ? dates[0].toDate() : null);
                                    setAbsenceDateTo(dates ? dates[1].toDate() : null);
                                }}
                                style={{ marginTop: '10px', width: '100%' }}
                            />
                            <Button className="removeAbsenceButton" type="primary" onClick={() => removeAbsence(index)} danger >
                                Remove
                            </Button>
                        </li>
                    ))}
                </ul>
            </Modal>

        </div>
    );
}