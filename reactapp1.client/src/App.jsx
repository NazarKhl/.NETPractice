import { useState, useEffect } from 'react';
import './App.css';
import { Modal, Button, Input, Checkbox, notification, DatePicker, Select, Pagination, Spin } from 'antd';
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
    const [currentPage, setCurrentPage] = useState(1);
    const [pageSize, setPageSize] = useState(10);
    const [totalUsers, setTotalUsers] = useState(0);
    const [visibleActivity, setVisibleActivity] = useState(false);
    const [sortConfig, setSortConfig] = useState({ key: null, direction: "" });
    const [idFilter, setIdFilter] = useState('');
    const [nameFilter, setNameFilter] = useState('');
    const [emailFilter, setEmailFilter] = useState('');


    useEffect(() => {
        showUsers(currentPage);
    }, [currentPage, pageSize, sortConfig, setIsAbsenceModalOpen]);

    const showUsers = async (page = 1) => {
        setLoading(true);
        try {
            const response = await fetch(`/api/user/paged?pageNumber=${page}&pageSize=${pageSize}&sortColumn=${sortConfig.key}&sortDirection=${sortConfig.direction}&idFilter=${idFilter}&nameFilter=${nameFilter}&emailFilter=${emailFilter}`);
            const data = await response.json();
            setUsers(data.users);
            setTotalUsers(data.totalCount);
            setCurrentPage(page);
        } catch (error) {
            notification.error({ description: error.message });
        } finally {
            setLoading(false);
        }
    };


    const handleIdFiterChange = (e) => {
  
        setIdFilter(e.target.value);
        showUsers(currentPage);
    };

    const handleNameFilterChange = (e) => {
        setNameFilter(e.target.value);
        showUsers(currentPage); 
    };

    const handleEmailFilterChange = (e) => {
        setEmailFilter(e.target.value);
        showUsers(currentPage);  
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
            const response = await fetch(`/api/absence/user/${user.id}`);
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
        setVisibleActivity(false);
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
            await showUsers(currentPage);
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
            await showUsers(currentPage);
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
            await showUsers(currentPage);
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
                    await showUsers(currentPage);
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
        const updatedAbsences = [...selectedUser.absences];
        const absenceId = selectedUser.absences[index].id;
        updatedAbsences.splice(index, 1);
        setSelectedUser(prev => ({
            ...prev,
            absences: updatedAbsences
        }));
        try {
            await fetch(`/api/absence/${absenceId}`, {
                method: 'DELETE',
            });
            notification.success({ message: 'Absence removed successfully' });
            await showUsers(currentPage);
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

    const showUserActivity = () => {
        setVisibleActivity(true);
    }

    const requestSort = (key) => {
        let direction = 'asc';
        if (sortConfig.key === key && sortConfig.direction === 'asc') {
            direction = 'desc';
        }
        setSortConfig({ key, direction });
    };

    const clearFields = () => {
        setIdFilter("");
        setNameFilter("");
        setEmailFilter("");
        showUsers();
    }

    const contents = loading
        ? <Spin size="large" />
        : filteredUsers.length === 0
            ? <p></p>
            : <table className="table table-striped" aria-labelledby="tableLabel">
                <thead>
                    <tr>
                        <th className="sortButtons" onClick={() => requestSort('id')}>Id</th>
                        <th className="sortButtons" onClick={() => requestSort('name')}>Name</th>
                        <th className="sortButtons" onClick={() => requestSort('email')}>Email</th>
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

            <div className="filterInputs">
                <Input
                    placeholder="Filter by ID"
                    value={idFilter}
                    onChange={handleIdFiterChange}
                    className="filterFields"
                />
                <Input
                    placeholder="Filter by Name"
                    value={nameFilter}
                    onChange={handleNameFilterChange}
                    className="filterFields"
                />
                <Input
                    placeholder="Filter by Email"
                    value={emailFilter}
                    onChange={handleEmailFilterChange}
                    className="filterFields"
                />
            </div>

            <Button onClick={() => showUsers(currentPage)} className="showUsers">Show users</Button><br/>
            <Button onClick={showCreateModal} className="createUser">Create New User</Button>
            <Button className="downloadJSON" onClick={downloadJSON}>Download .json</Button>
            <Button onClick={showUserActivity} className="userActivityCharrt">User Activity</Button>
            <Button onClick={clearFields} className="clearFields">Clear Fields</Button><br/>
            {contents}
            <Pagination
                current={currentPage}
                pageSize={pageSize}
                total={totalUsers}
                onChange={(page, pageSize) => {
                    setCurrentPage(page);
                    setPageSize(pageSize);
                }}
                showQuickJumper
                showSizeChanger
                className="paginationPanel"
            />
            <Modal
                title="Create User"
                visible={isCreateModalOpen}
                onOk={handleCreateUser}
                onCancel={hideModals}
            >
                <Input
                    className="inputChanger"
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
                    className="inputChanger"
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
                title="Add Absence"
                visible={isAbsenceModalOpen}
                onOk={handleAbsenceAdd}
                onCancel={hideModals}
            >
                <Select
                    value={absenceType}
                    onChange={value => setAbsenceType(value)}
                    placeholder="Select Absence Type"
                    className="selectAbsenceType"
                >
                    <Option value={1}>Illness</Option>
                    <Option value={2}>Vacation</Option>
                    <Option value={3}>Other</Option>
                </Select>
                <Input
                    value={absenceDescription}
                    onChange={e => setAbsenceDescription(e.target.value)}
                    placeholder="Description"
                    className="absenceDescription"
                />
                <RangePicker
                    value={[absenceDateFrom ? moment(absenceDateFrom) : null, absenceDateTo ? moment(absenceDateTo) : null]}
                    onChange={(dates) => {
                        setAbsenceDateFrom(dates ? dates[0].toDate() : null);
                        setAbsenceDateTo(dates ? dates[1].toDate() : null);
                    }}
                    className="rangePicker"
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
                                disabled
                                value={absenceType !== null ? absenceType : absence.type}
                                onChange={value => setAbsenceType(value)}
                                placeholder="Select Absence Type"
                                className="absenceSelectType"
                            >
                                <Option value={1}>Illness</Option>
                                <Option value={2}>Vacation</Option>
                                <Option value={3}>Other</Option>
                            </Select>
                            <Input
                                disabled
                                value={absenceDescription || absence.description}
                                onChange={e => setAbsenceDescription(e.target.value)}
                                placeholder="Description"
                                className="inputAbsenceDescription"
                            />
                            <RangePicker
                                disabled
                                value={[
                                    absenceDateFrom ? moment(absenceDateFrom) : moment(absence.dateFrom),
                                    absenceDateTo ? moment(absenceDateTo) : moment(absence.dateTo)
                                ]}
                                onChange={(dates) => {
                                    setAbsenceDateFrom(dates ? dates[0].toDate() : null);
                                    setAbsenceDateTo(dates ? dates[1].toDate() : null);
                                }}
                                className="absenceRangePicker"
                            />
                            <Button className="removeAbsenceButton" type="primary" onClick={() => removeAbsence(index)} danger >
                                Remove
                            </Button>
                        </li>
                    ))}
                </ul>
            </Modal>
            <Modal
                visible={visibleActivity}
                onCancel={hideModals}
                footer={null}
            >
                <UserChart users={users} />
            </Modal>
        </div>
    );
}