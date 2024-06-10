import axios from 'axios'
import { BaseClient } from './BaseClient'
export class AuthClient extends BaseClient {
  public async Login(user: string, pass: string): Promise<string> {
    try {
      const resp = await axios.post(this.baseUrl + '/auth', {
        username: user,
        password: pass
      })
      return resp.data.token
    } catch (error: any) {
      if (error.response) {
        throw new Error(`Failed to fetch data: "${error.response.data}"`)
      }
      throw new Error(`Failed to fetch data: ${error}`)
    }
  }

  public async Register(
    user: string,
    pass: string,
    email: string,
    firstname: string,
    lastname: string
  ): Promise<string> {
    try {
      const resp = await axios.put(this.baseUrl + '/auth', {
        email: email,
        userName: user,
        firstName: firstname,
        lastName: lastname,
        photoContentType: 'string',
        //photoData: 'string',
        password: pass,
        role: 'User'
      })
      return ''
    } catch (error: any) {
      if (error.response) {
        throw new Error(`Failed to fetch data: "${error.response.data}"`)
      }
      throw new Error(`Failed to fetch data: ${error}`)
    }
  }
}
