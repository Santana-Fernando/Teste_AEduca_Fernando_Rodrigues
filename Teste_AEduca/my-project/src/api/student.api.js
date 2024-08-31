import api from '@/api'

export default {
  list: () => api.get('/Students/GetList').then((response) => response.data),
  get: (ra) =>
    api.get(`/Students/GetById?ra=${ra}`).then((response) => response.data),
  create: (student) =>
    api.post('/Students/Register', student).then((response) => response),
  update: (student) =>
    api.put(`/Students/Update`, student).then((response) => response),
  delete: (ra) =>
    api.delete(`/Students/Remove?ra=${ra}`).then((response) => response),
}
