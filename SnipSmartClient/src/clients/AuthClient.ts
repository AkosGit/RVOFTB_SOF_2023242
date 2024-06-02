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
    } catch (error) {
      throw new Error(`Failed to fetch data: ${error}`)
    }
  }
}
